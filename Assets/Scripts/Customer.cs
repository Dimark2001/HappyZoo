using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    [SerializeField] private float _payingTime = 3f;
    [SerializeField] private float _watchingTime = 10f;
    [SerializeField] private GameObject _money;
    public Transform homePosition;
    public bool IsLooking = false;
    
    public List<CustomersZone> _allCustomersZones = new List<CustomersZone>();
    private List<CustomersZone> _availableZones => _allCustomersZones.FindAll(x => x.IsWork && !_passedZones.Contains(x));
    private List<CustomersZone> _passedZones = new List<CustomersZone>();

    [SerializeField] private CustomersZone _currentZone;
    [SerializeField] private NavMeshAgent _agent;

    private void Start()
    {
        if (_availableZones.Count == 0)
        {
            GoHome();
            return;
        }

        FindOpenPaddock();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CashRegister cashRegister))
        {
            if (_currentZone.IsWork)
            {
                StopAllCoroutines();
                StartPaying(_currentZone);
            }
            else
            {
                StartCoroutine(WaitForAnimals());
            }
        }
    }

    private IEnumerator WaitForAnimals()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (_currentZone.IsWork)
        {
            StartPaying(_currentZone);
        }
        else
        {
            StartCoroutine(WaitForAnimals());
        }
    }

    private void FindOpenPaddock()
    {
        StopAllCoroutines();
        var position = new Vector3();
        if (_availableZones.Count == 0)
        {
            GoHome();
            return;
        }
        _currentZone = _availableZones[0];
        if (_currentZone.TryGetLastPosition(out position))
        {
            _passedZones.Add(_currentZone);

            if (!TryGoTo(position))
            {
                GoHome();
                Debug.LogWarning("Не может добраться до кассы!!");
            }
            else
            {
                _currentZone._customersInQueue.Add(this);
                PeekNextPosition();
            }
        }
        else
        {
            GoHome();
        }
    }

    private bool TryGoTo(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        if (_agent.CalculatePath(position, path))
        {
            _agent.SetPath(path);
            return true;
        }

        return false;
    }

    private void StartPaying(CustomersZone cz)
    {
        StartCoroutine(Pay(cz));
    }
    
    private void GoHome()
    {
        TryGoTo(homePosition.position);
    }

    private void PeekNextPosition()
    {
        StartCoroutine(PeekNextPositionCoroutine());
    }

    private void SpawnMoney()
    {
        var side = transform.position.x < 0 ? 1 : -1;
        var money = Instantiate(_money, transform.position + Vector3.right * side * 5, Quaternion.identity);
    }

    private IEnumerator PeekNextPositionCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        
        try
        {
            int index = _currentZone._customersInQueue.FindIndex(x => x == this); 

            if (index != -1)
            {
                TryGoTo(_currentZone._queue[index].position);
            }
        }
        catch
        {
            Debug.Log("Oy");
        }
        
        StartCoroutine(PeekNextPositionCoroutine());
    }
    
    private IEnumerator Pay(CustomersZone customersZone)
    {
        yield return new WaitForSeconds(_payingTime);
        customersZone._customersInQueue.Remove(this);
        
        SpawnMoney();
        if (customersZone.TryGetFreeLookingZone(out Vector3 position))
        {
            TryGoTo(position);

            var index = customersZone.AddToLookingZone(this);

            IsLooking = true;

            for (int i = 0; i < 8; i++)
            {
                yield return new WaitForSeconds(_watchingTime/8);
                if (_agent.velocity.magnitude <= 0.1f)
                {
                    DOTween.To(() => transform.forward, x => transform.forward = x, _currentZone.direction, 0.5f);
                }
                if (!_currentZone.IsWork)
                {
                    break;
                }
            }
            
            FindOpenPaddock();
            
            customersZone._customersAtLookingPoints[index] = null;
            yield break;
        }
     
        Debug.Log("Не может найти зону для просмотра");
        FindOpenPaddock();
    }
}
