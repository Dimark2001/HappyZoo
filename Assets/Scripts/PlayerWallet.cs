using System;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField] private int _startMoney = 5000;
    [SerializeField] private bool _isInfinite;

    public int MoneyCount
    {
        get => moneyCount;
        set
        {
            if (value < 0)
                moneyCount = 0;

            moneyCount = value;
            OnMoneyCountChanged?.Invoke(value);
            
            PlayerPrefs.SetInt("moneyCount", moneyCount);
            PlayerPrefs.Save();
        }
    }
    public Action<int> OnMoneyCountChanged;

    private int moneyCount;

    private void Start()
    {
        MoneyCount = PlayerPrefs.GetInt("moneyCount", _startMoney);
        if (_isInfinite)
        {
            MoneyCount = 999999999;
        }
    }
}
