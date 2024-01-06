using UnityEngine;
using System;
using System.Collections;
using Agava.YandexGames;
using DG.Tweening;
using NaughtyAttributes;

public class YandexSdk : MonoBehaviour
{
    public static YandexSdk Instance;
    public Action OnSdkInit;
    public Action OnAdsShow;
    public float time = 61f;
    public Localisation CurrentLocalization { get; private set; }
    public enum Localisation
    {
        Ru,
        En,
        Tr
    }

    [SerializeField] private string localization = "ru";
    private bool _isInit;

    public void WaitForYandexSdkInit(Action onSdkInit)
    {
        if (_isInit)
        {
            onSdkInit?.Invoke();
        }
        else
        {
            OnSdkInit += () =>
            {
                onSdkInit?.Invoke();
            };
        }
    }

    [Button()]
    private void DeleteAllSave()
    {
        PlayerPrefs.DeleteAll();
    }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _isInit = false;
        
#if UNITY_WEBGL && !UNITY_EDITOR
        var initialize = YandexGamesSdk.Initialize(OnInitSuccess);
        StartCoroutine(initialize);
#else
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 0, 2f).OnComplete((() =>
        {
            OnInitSuccess();
        }));
#endif
        
        void OnInitSuccess()
        {
            Debug.Log("-------------_init------------");
            var inVal = 0f;
            DOTween.To(() => inVal, x => inVal = x, 0, 1f).OnComplete((() =>
            {
                _isInit = true;
                CheckLocalization();
                OnSdkInit?.Invoke();
                ShowAddForTime();
                
            }));
        }
    }

    private void CheckLocalization()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        localization = YandexGamesSdk.Environment.i18n.lang;
#endif
        switch (localization)
        {
            case "ru":
                CurrentLocalization = Localisation.Ru;
                break;
            case "en":
                CurrentLocalization = Localisation.En;
                break;
            case "tr":
                CurrentLocalization = Localisation.Tr;
                break;
            default:
                CurrentLocalization = Localisation.En;
                break;
        }

        Debug.Log("current lang = " + localization + " current localization = " + CurrentLocalization);
    }

    private void ShowAddForTime()
    {
        StopAllCoroutines();
        var showAds = ShowAds();
        StartCoroutine(showAds);

        IEnumerator ShowAds()
        {
            while (true)
            {
                yield return new WaitForSeconds(time);
                Debug.Log("OnAdsShow?.Invoke");
                OnAdsShow?.Invoke();
                yield break;
            }
        }
    }

    public void ShowInterstitialAd()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        InterstitialAd.Show(OnAdsOpen, OnAdsClose, OnAdsError);
#else
        OnAdsOpen();
        OnAdsClose(true);
#endif
        void OnAdsOpen()
        {
            Time.timeScale = 0;
            Debug.Log("OnAdsOpen");
        }

        void OnAdsClose(bool isOpen)
        {
            Time.timeScale = 1;
            ShowAddForTime();
            Debug.Log("OnAdsClose" + isOpen);
        }

        void OnAdsError(string error)
        {
            Time.timeScale = 1;
            Debug.Log("OnAdsError" + "||" + error + "||");
        }
    }

    /*public void AddProduct(PurchasedProduct product)
    {
        purchasedProducts.Add(product);
    }*/

    /*public void BuyStars(string id, int count)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        OnStarsBuyComplete -= Buy;
        OnStarsBuyComplete += Buy;
        _onStarsBuyError -= Error;
        _onStarsBuyError += Error;

        Billing.PurchaseProduct(id, OnStarsBuyComplete, _onStarsBuyError);
#else
        Debug.Log("OnStarsBuyComplete: " + id + "Buy stars: " + count);
        LevelManager.Instance.gameBoardWindow.playerCiv.AddMoney(count);
#endif
        void Buy(PurchaseProductResponse response)
        {
            OnStarsBuyComplete -= Buy;
            _onStarsBuyError -= Error;
            Debug.Log("OnStarsBuyComplete: " + id + "Buy stars: " + count);
            LevelManager.Instance.gameBoardWindow.playerCiv.AddMoney(count);
        }

        void Error(string error)
        {
            _onStarsBuyError -= Error;
            OnStarsBuyComplete -= Buy;
            Debug.Log("_onStarsBuyError: " + error);
        }
    }*/
    
    /*public void BuyStarsAndBlockAds(string id, int count)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        OnStarsBuyComplete -= Buy;
        OnStarsBuyComplete += Buy;
        _onStarsBuyError -= Error;
        _onStarsBuyError += Error;

        Billing.PurchaseProduct(id, OnStarsBuyComplete, _onStarsBuyError);
#else
        Debug.Log("OnStarsBuyComplete: " + id + "Buy stars: " + count);
        isHaveAds = false;
#endif
        void Buy(PurchaseProductResponse response)
        {
            OnStarsBuyComplete -= Buy;
            _onStarsBuyError -= Error;
            Debug.Log("OnStarsBuyComplete: " + id + "Buy stars: " + count);
#if UNITY_WEBGL && !UNITY_EDITOR
            AddProduct(response.purchaseData);
            PlayerPrefs.Save();
#endif
            isHaveAds = false;
        }

        void Error(string error)
        {
            _onStarsBuyError -= Error;
            OnStarsBuyComplete -= Buy;
            Debug.Log("_onStarsBuyError: " + error);
        }
    }*/
    
    /*public void BuyATribe(string id)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Billing.PurchaseProduct(id, OnTribeBuyComplete, OnTribeBuyError);
        PlayerPrefs.Save();
#else
        Debug.Log("OnStarsBuyComplete: " + id);
        purchasedProductsInEditor ??= new List<string>();
        purchasedProductsInEditor.Add(id);
        OnTribeBuyComplete?.Invoke(null);
#endif
    }*/
    
    /*public void SaveGameProgress(string cloudSaveDataJson, Action onSave)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerAccount.SetCloudSaveData(cloudSaveDataJson, onSave);
#endif
    }*/

    public void ShowRewardAds(Action onOpenCallback, Action onRewardedCallback, Action onCloseCallback, Action<string> onErrorCallback)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        VideoAd.Show(onOpenCallback, onRewardedCallback, onCloseCallback, onErrorCallback);
#else
        onRewardedCallback?.Invoke();
#endif
    }
}
