using System.Collections;
using System.Linq;
using UnityEngine;

public class ChildsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _child;
    [SerializeField] private float _minSpawningInterval = 0.5f;
    [SerializeField] private float _maxSpawningInterval = 3f;
    [SerializeField] private int _maxCount = 15;
    [SerializeField] private Transform _spawningPoint;
    [SerializeField] private CustomersZone[] _allCustomersZones;

    private float _waitTime => Random.Range(_minSpawningInterval, _maxSpawningInterval);
    
    private int _currentCount = 0;

    private void Start()
    {
        StartCoroutine(SpawningLoop());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Customer customer))
        {
            Destroy(customer.gameObject);
            _currentCount--;
        }
    }

    private IEnumerator SpawningLoop()
    {
        yield return new WaitForSeconds(_waitTime);

        if (_currentCount < _maxCount)
        {
            Customer customer = Instantiate(_child, _spawningPoint.position, Quaternion.identity).GetComponent<Customer>();
            customer.homePosition = transform;
            customer._allCustomersZones = _allCustomersZones.ToList();
            _currentCount++;
        }

        StartCoroutine(SpawningLoop());
    }
}
