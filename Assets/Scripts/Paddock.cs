using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

// Активировать зону выгрузки пищи
// Менять состояние на купленное
// Отправлять слонам сигналы для анимации и сигнал для убегания в загон
// Показывать наружу, что здесь можно потратить деньги
// 
// **ПИЩЕВОЙ КАМЕНЬ ОБЪЕДИНЯЕТСЯ С ЗАГОНОМ!!! УРА!!!**
// Нужен класс выгрузки пищи (пищевой камень). Он будет раз в минуту кричать о том, что нужно от 4 до 10 указанной пищи,
// а также сообщать загону, что пора слонам спрятаться.
//
// Нужен класс покупки зон.
// Он будет активировать загон, когда игрок его купит.
//
// Покупаем загон -> он активируется -> слоны начинают бегать -> пищевой камень начинает отсчет минуты ->
// дети бегут покупать билеты и смотреть на слонов -> камень досчитывает -> посылает сигнал загонять слонов -> дети больше не идут.

[RequireComponent(typeof(Collider), typeof(PaddockStack))]
public class Paddock : MonoBehaviour
{
    public Action CountOfResourceChanged;
    public Action OnAte;
    public Action OnFood;
    public Action OnGetHungry;
    public PaddockStack paddockStack;
    public int CountOfResource => _countOfResource;

    [SerializeField] private CustomersZone _customersZone;

    [SerializeField] private Resource _necessaryResource;
    [SerializeField] private int _countOfResource = 5;

    [SerializeField] private int _minCountOfResource = 4;
    [SerializeField] private int _maxCountOfResource = 10;
    
    [SerializeField] private float _minSatietyTime = 50f;
    [SerializeField] private float _maxSatietyTime = 70f;
    [SerializeField] private float _takingTime = 1f;
    [SerializeField] private Transform _foodTakerPosition;
    
    private bool _isWantToEat = false;
    private TweenerCore<Vector3, Vector3, VectorOptions> _tweenerCore;


    private void Awake()
    {
        paddockStack = GetComponent<PaddockStack>();
    }

    private void Start()
    {
        StartCoroutine(SatietyLoop());
    }

    private void OnEnable()
    {
        OnAte += RestartSatiety;
    }

    private void OnDisable()
    {
        OnAte -= RestartSatiety;
    }

    private void RestartSatiety()
    {
        StartCoroutine(SatietyLoop());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ResourcesStack resourcesStack))
        {
            if (_countOfResource > 0 && _isWantToEat)
            {
                TakeFood(resourcesStack);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ResourcesStack resourcesStack))
        {
            _tweenerCore.Kill();
        }
    }

    private void TakeFood(ResourcesStack resourcesStack)
    {
        Resource resource = resourcesStack.Resources.Find(x => x.Prefab == _necessaryResource.Prefab);
        if (resource == null)
            return;
        
        Resource res = Instantiate(_necessaryResource);
        
        res.transform.position = resource.transform.position;

        var position = _foodTakerPosition.position;
                
        var scaling = res.transform.DOScale(Vector3.one, _takingTime);
        var rotating = res.transform.DORotateQuaternion(resourcesStack.transform.rotation, _takingTime);

        _tweenerCore = res.transform.DOMove(position, _takingTime);
        _tweenerCore
            .OnComplete(() =>
            {
                bool result = resourcesStack.TryRemoveResource(res);
                if (result)
                {
                    _countOfResource--;
                    CountOfResourceChanged?.Invoke();
                    paddockStack.AddResource(res);

                    if (_countOfResource == 0)
                    {
                        OnAte?.Invoke();
                        _customersZone.IsWork = true;
                        _isWantToEat = false;
                        _tweenerCore.Kill();
                        return;
                    }

                    TakeFood(resourcesStack);
                }
            })
            .OnKill(() =>
            {
                scaling.Kill();
                rotating.Kill();
                Destroy(res.gameObject);
            });
        _tweenerCore.SetEase(Ease.InQuad);
    }

    private void GetHungry()
    {
        _countOfResource = Random.Range(_minCountOfResource, _maxCountOfResource);
        CountOfResourceChanged?.Invoke();
        _customersZone.IsWork = false;
        OnGetHungry?.Invoke();
    }

    IEnumerator SatietyLoop()
    {
        yield return new WaitForSeconds(Random.Range(_minSatietyTime, _maxSatietyTime));

        if (!_isWantToEat)
        {
            _isWantToEat = true;
            GetHungry();
            StopCoroutine(SatietyLoop());
            yield break;
        }

        StartCoroutine(SatietyLoop());
    }
}