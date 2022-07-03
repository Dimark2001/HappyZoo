using System;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

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
        if (other.TryGetComponent(out _wallet))
        {
             DOTween.To(() => _camera.m_Lens.FieldOfView, x => _camera.m_Lens.FieldOfView = x, 50, 0.5f);
            
            var money = _wallet.MoneyCount;
            var time = _openingTime * (Mathf.Min(money, _currentCost) / (float)_totalCost);
     
            _tweener = DOTween.To(() => _currentCost, x => _currentCost = x,
                    Mathf.Max(_currentCost - _wallet.MoneyCount, 0), time)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (_currentCost == 0)
                    {
                        PlayerPrefs.SetInt(_zoneId.ToString(), _currentCost);
                        PlayerPrefs.Save();
                        Buy();
                        DOTween.To(() => _camera.m_Lens.FieldOfView, x => _camera.m_Lens.FieldOfView = x, 60, 0.5f);
                    }
                }).OnUpdate(UpdateLabel);
            _tweener2 = DOTween.To(() => _wallet.MoneyCount, x => _wallet.MoneyCount = x,
                Mathf.Max(_wallet.MoneyCount - _currentCost, 0), time).SetEase(Ease.Linear);
         
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out _wallet))
        {
            _tweener.Kill();
            _tweener2.Kill();
            
            DOTween.To(() => _camera.m_Lens.FieldOfView, x => _camera.m_Lens.FieldOfView = x, 60, 0.5f);
            PlayerPrefs.SetInt(_zoneId.ToString(), _currentCost);
            PlayerPrefs.Save();
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
}
