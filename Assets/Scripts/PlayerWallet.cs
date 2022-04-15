using System;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
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
        MoneyCount = PlayerPrefs.GetInt("moneyCount", 5000);
    }
}
