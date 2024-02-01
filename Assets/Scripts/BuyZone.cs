using System;
using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Unity.Mathematics;

[RequireComponent(typeof(Collider))]
public class BuyZone : MonoBehaviour
{
    public Action OnBought;
    
    [SerializeField] private int _zoneId = 0;

    [SerializeField] private float _openingTime = 3f;
    [SerializeField] private int _totalCost;
    [SerializeField] private int _currentCost;
    
    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshPro _countLabel;
    [SerializeField] private GameObject _toActivate;
    [SerializeField] private GameObject _toDeactivate;

    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private GameObject[] _condition;

    private int _boughtConditions = 0;
    private PlayerWallet _wallet;
    private TweenerCore<int, int, NoOptions> _tweener;
    private TweenerCore<int, int, NoOptions> _tweener2;
    private float _progress => _currentCost / (float)_totalCost;

    private bool _isBuying;

    private void Awake()
    {
        if (_condition == null || _condition.Length == 0 || _condition[0] == null) return;
        foreach (var cond in _condition)
        {
            cond.GetComponent<BuyZone>().OnBought += OnConditionBought;
        }
    }

    private void Start()
    {
        _currentCost = PlayerPrefs.GetInt(_zoneId.ToString(), _totalCost);
        UpdateLabel();
        if (_currentCost == 0)
        {
            Buy();
        }

        if (_condition == null || _condition.Length == 0 || _condition[0] == null)
        {
            Activate();
        }
    }

    private void OnConditionBought()
    {
        _boughtConditions++;

        if (_boughtConditions == _condition.Length)
        {
            Activate();
        }
    }

    private void Activate()
    {
        GetComponent<Collider>().enabled = true;
        GetComponentInChildren<Canvas>().enabled = true;
        _countLabel.enabled = true;
    }

    private void OnDestroy()
    {
        if (_condition == null || _condition.Length == 0 || _condition[0] == null) return;
        foreach (var cond in _condition)
        {
            cond.GetComponent<BuyZone>().OnBought -= OnConditionBought;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerWallet wallet))
        {
             DOTween.To(() => _camera.m_Lens.FieldOfView, x => _camera.m_Lens.FieldOfView = x, 50, 0.5f);

             _isBuying = true;
             _wallet = wallet;
             StartCoroutine(BuyingCoroutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerWallet wallet))
        {
            _isBuying = false;
            PlayerPrefs.SetInt(_zoneId.ToString(), _currentCost);
            PlayerPrefs.Save();
            DOTween.To(() => _camera.m_Lens.FieldOfView, x => _camera.m_Lens.FieldOfView = x, 60, 0.5f);
        }
    }
    
    private string FormatMoneyCount(int count)
    {
        string formattedCount = count > 1000 ? "$" + (Mathf.FloorToInt((float)count / 100) / 10f) + "K" : "$" + count;
        return formattedCount;
    }

    private void UpdateLabel()
    {
        _countLabel.text = FormatMoneyCount(_currentCost);
        _progressBar.fillAmount = 1f - _progress;
    }

    private void Buy()
    {
        if (_toActivate != null)
            _toActivate.SetActive(true);
        
        if (_toDeactivate != null)
            _toDeactivate.SetActive(false);
        OnBought?.Invoke();
        gameObject.SetActive(false);
    }

    private IEnumerator BuyingCoroutine()
    {
        var startMoney = _wallet.MoneyCount;
        var startCost = _currentCost;
        var endMoney = Mathf.Max(startMoney - startCost, 0);
        var endCost = Mathf.Max(startCost - startMoney, 0);
        var time = (float)(startCost - endCost)/_totalCost * _openingTime;

        var timer = 0f;
        var error = 0d;
        while (true)
        {
            if (_isBuying == false)
            {
                yield break;
            }
            if (timer >= time)
            {
                break;
            }

            timer += Time.deltaTime;

            var step = (startCost - endCost) * (Time.deltaTime / time);
            error += Math.Truncate(step);
            var intStep = Mathf.FloorToInt(step);
            
            if (error >= 1)
            {
                var errorInt = Mathf.FloorToInt((float)error);
                intStep += errorInt;
                error -= errorInt;
            }

            if (_wallet != null)
            {
                _wallet.MoneyCount -= intStep;
                _currentCost -= intStep;

                if (_currentCost <= 0)
                {
                    _currentCost = 0;
                    _wallet.MoneyCount = endMoney;
                    PlayerPrefs.SetInt(_zoneId.ToString(), _currentCost);
                    PlayerPrefs.Save();
                    Buy();
                    yield break;
                }

                if (_wallet.MoneyCount <= 0)
                {
                    _currentCost = endCost;
                    _wallet.MoneyCount = 0;
                    PlayerPrefs.SetInt(_zoneId.ToString(), _currentCost);
                    PlayerPrefs.Save();
                    UpdateLabel();
                    yield break;
                }
            }
            else
            {
                Debug.Log("walletNull");
            }

            UpdateLabel();
            
            yield return null;
        }

        if (_wallet != null)
        {
            _wallet.MoneyCount = endMoney;
            _currentCost = endCost;

            if (_currentCost <= 0)
            {
                _currentCost = 0;
                _wallet.MoneyCount = endMoney;
                PlayerPrefs.SetInt(_zoneId.ToString(), _currentCost);
                PlayerPrefs.Save();
                Buy();
                yield break;
            }
        }
        else
        {
            Debug.Log("walletNull");
        }

        PlayerPrefs.SetInt(_zoneId.ToString(), _currentCost);
        PlayerPrefs.Save();
        UpdateLabel();
    }
}
