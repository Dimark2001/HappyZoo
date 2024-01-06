using UnityEngine;

public class YandexWaitScreen : MonoBehaviour
{
    private void Start()
    {
        YandexSdk.Instance.WaitForYandexSdkInit((() =>
        {
            gameObject.SetActive(false);
        }));
    }
}
