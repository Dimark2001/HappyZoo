using UnityEngine;

public class MoneyPack : MonoBehaviour
{
    public int Amount = 5;
    public bool IsPickable = false;
    private void OnTriggerEnter(Collider other)
    {
        if (IsPickable == false)
            return;

        PlayerWallet playerWallet;
        if (other.TryGetComponent(out playerWallet))
        {
            IsPickable = false;
            playerWallet.MoneyCount += Amount;
            
            LeanTween.move(gameObject, playerWallet.transform.position, 0.2f).setEaseInCubic().setOnComplete(Destroy);
            LeanTween.scale(gameObject, Vector3.zero, 0.2f);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
