using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerWallet))]
public class PlayerWalletView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    private PlayerWallet _playerWallet;

    private void Awake()
    {
        _playerWallet = GetComponent<PlayerWallet>();
    }

    private void OnEnable()
    {
        _playerWallet.OnMoneyCountChanged += UpdateMoneyLabel;
    }

    private void UpdateMoneyLabel(int moneyCount)
    {
        label.text = moneyCount.ToString();
    }
}
