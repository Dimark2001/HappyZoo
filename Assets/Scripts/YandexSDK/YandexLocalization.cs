using TMPro;
using UnityEngine;

public class YandexLocalization : MonoBehaviour
{
    [SerializeField] private string ru;
    [SerializeField] private string en;
    [SerializeField] private string tr;
    void Start()
    {
        var tmPro = GetComponent<TextMeshProUGUI>();
        if(tmPro == null)
            return;
        YandexSdk.Instance.WaitForYandexSdkInit(() =>
        {
            switch (YandexSdk.Instance.CurrentLocalization)
            {
                case YandexSdk.Localisation.Ru:
                    tmPro.text = ru;
                    break;
                case YandexSdk.Localisation.En:
                    tmPro.text = en;
                    break;
                case YandexSdk.Localisation.Tr:
                    tmPro.text = tr;
                    break;
                default:
                    tmPro.text = "Ошибка";
                    break;
            }
        });
    }
}
