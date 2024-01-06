using System.Collections;
using UnityEngine;
using TMPro;

public class AdsForTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private void Start()
    {
        YandexSdk.Instance.WaitForYandexSdkInit((() =>
        {
            timerText.gameObject.SetActive(false);
            YandexSdk.Instance.OnAdsShow += ShowAds;
        }));
    }
    
    private void ShowAds()
    {
        Debug.Log("YandexSdk.Instance.OnAdsShow");
        timerText.gameObject.SetActive(true);
        var corutine = Show();
        StartCoroutine(corutine);
    }
    
    private IEnumerator Show()
    {
        while (true)
        {
            switch (YandexSdk.Instance.CurrentLocalization)
            {
                case YandexSdk.Localisation.Ru:
                    timerText.text = "Реклама через " +
                                     "3...";
                    break;
                case YandexSdk.Localisation.En:
                    timerText.text = "Advertising in " +
                                     "3...";
                    break;
                case YandexSdk.Localisation.Tr:
                    timerText.text = "3'te reklam...";
                    break;
                default:
                    timerText.text = "Advertising in " +
                                     "3...";
                    break;
            }
            
            yield return new WaitForSecondsRealtime(1);
            switch (YandexSdk.Instance.CurrentLocalization)
            {
                case YandexSdk.Localisation.Ru:
                    timerText.text = "Реклама через " +
                                     "2...";
                    break;
                case YandexSdk.Localisation.En:
                    timerText.text = "Advertising in " +
                                     "2...";
                    break;
                case YandexSdk.Localisation.Tr:
                    timerText.text = "2'te reklam...";
                    break;
                default:
                    timerText.text = "Advertising in " +
                                     "2...";
                    break;
            }
            yield return new WaitForSecondsRealtime(1);
            switch (YandexSdk.Instance.CurrentLocalization)
            {
                case YandexSdk.Localisation.Ru:
                    timerText.text = "Реклама через " +
                                     "1...";
                    break;
                case YandexSdk.Localisation.En:
                    timerText.text = "Advertising in " +
                                     "1...";
                    break;
                case YandexSdk.Localisation.Tr:
                    timerText.text = "1'te reklam...";
                    break;
                default:
                    timerText.text = "Advertising in " +
                                     "1...";
                    break;
            }
            yield return new WaitForSecondsRealtime(1);
            YandexSdk.Instance.ShowInterstitialAd();
            timerText.gameObject.SetActive(false);
            yield break;
        }
    }
    
    
}
