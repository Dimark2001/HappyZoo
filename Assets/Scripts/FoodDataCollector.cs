using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FoodDataCollector : MonoBehaviour
{
    [SerializeField] private Paddock[] _grassPaddocks;
    [SerializeField] private Paddock[] _fishPaddocks;
    [SerializeField] private Paddock[] _meatPaddocks;

    [SerializeField] private GameObject _grassIndicator;
    [SerializeField] private GameObject _fishIndicator;
    [SerializeField] private GameObject _meatIndicator;

    [SerializeField] private TextMeshProUGUI _grassLabel;
    [SerializeField] private TextMeshProUGUI _fishLabel;
    [SerializeField] private TextMeshProUGUI _meatLabel;

    private int _grassCount = 0;
    private int _fishCount = 0;
    private int _meatCount = 0;

    private void Start()
    {
        UpdateView();
    }

    private void OnEnable()
    {
        Subscribe(_grassPaddocks);
        Subscribe(_fishPaddocks);
        Subscribe(_meatPaddocks);
    }

    private void OnDisable()
    {
        Unsubscribe(_grassPaddocks);
        Unsubscribe(_fishPaddocks);
        Unsubscribe(_meatPaddocks);
    }

    private void Subscribe(Paddock[] paddocks)
    {
        foreach (var paddock in paddocks)
        {
            paddock.CountOfResourceChanged += UpdateData;
        }
    }
    
    private void Unsubscribe(Paddock[] paddocks)
    {
        foreach (var paddock in paddocks)
        {
            paddock.CountOfResourceChanged -= UpdateData;
        }
    }

    private void UpdateData()
    {
        _grassCount = CountResource(_grassPaddocks);
        _fishCount = CountResource(_fishPaddocks);
        _meatCount = CountResource(_meatPaddocks);
        UpdateView();
    }

    private void UpdateView()
    {
        if (!_fishIndicator.activeSelf && _fishPaddocks.Any(x => x.gameObject.activeInHierarchy))
        {
            _fishIndicator.SetActive(true);
        }

        if (!_meatIndicator.activeSelf && _meatPaddocks.Any(x => x.gameObject.activeInHierarchy))
        {
            _meatIndicator.SetActive(true);
        }
        
        _grassLabel.text = _grassCount.ToString();
        _fishLabel.text = _fishCount.ToString();
        _meatLabel.text = _meatCount.ToString();
    }

    private int CountResource(Paddock[] paddocks)
    {
        int result = 0;
        
        foreach (var paddock in paddocks)
        {
            if (paddock.isActiveAndEnabled)
            {
                result += paddock.CountOfResource;
            }
            
        }

        return result;
    }

}
