using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomersZone : MonoBehaviour
{
    public bool IsWork = false;
    
    [SerializeField] public List<Transform> _queue = new List<Transform>();
    [SerializeField] private List<Transform> _lookingPoints = new List<Transform>();

    public List<Customer> _customersInQueue = new List<Customer>();
    public List<Customer> _customersAtLookingPoints = new List<Customer>();

    // НОВЫХ ПОСЕТИТЕЛЕЙ ДОЛЖЕН СОЗДАВАТЬ СПАУНЕР ОН ЖЕ БУДЕТ ОПРАШИВАТЬ КАСТОМЕР ЗОНЫ НА ТО НУЖНЫ ЛИ НОВЫЕ ПОСЕТИТЕЛИ И НЕ ДОСТИГЛИ ЛИ МЫ МАКСИМУМА

    private void Awake()
    {
        for (int i = 0; i < _lookingPoints.Count; i++)
        {
            _customersAtLookingPoints.Add(null);
        }
    }

    public bool TryGetLastPosition(out Vector3 position)
    {
        if (_customersInQueue.Count < _queue.Count)
        {
            position = _queue[_customersInQueue.Count].position;
            return true;
        }

        position = default;
        return false;
    }

    public int AddToLookingZone(Customer customer)
    {
        for (int i = 0; i < _customersAtLookingPoints.Count; i++)
        {
            var c = _customersAtLookingPoints[i];
            if (c == null)
            {
                _customersAtLookingPoints[i] = customer;
                return i;
            }
        }

        return 0;
    }

    public bool TryGetFreeLookingZone(out Vector3 position)
    {
        for (int i = 0; i < _lookingPoints.Count; i++)
        {
            var c = _customersAtLookingPoints[i];
            if (c == null)
            {
                position = _lookingPoints[i].position;
                return true;
            }
        }
        position = default;
        return false;
    }

    private void OnEnable()
    {
        IsWork = true;
    }
}
