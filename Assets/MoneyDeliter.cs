using UnityEngine;

public class MoneyDeliter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MoneyPack money))
        {
            Debug.Log("Money removed");
            money.gameObject.SetActive(false);
        }
    }
}
