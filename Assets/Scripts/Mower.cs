using System;
using UnityEngine;

public class Mower : MonoBehaviour
{
    [SerializeField] private GameObject _grassBlock;
    [SerializeField] private int _grassInOneBlock = 100;
    
    private bool _isActive;
    private SphereCollider _sphereCollider;
    private Grass _grass;
    private int _mowedCount;
    
    public Action OnActivate;
    public Action OnDeactivate;
    public Action<int> OnCollideGrass;
    
    public bool IsActive => _isActive;
    public bool IsNotActive => !_isActive;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grass"))
        {
            other.gameObject.SetActive(false);
            return;
        }
        if (other.TryGetComponent(out Grass grass))
        {
            _isActive = true;
            OnActivate?.Invoke();
            _grass = grass;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out GrassMowingZone zone))
        {
            _isActive = false;
            OnDeactivate?.Invoke();
            _grass = null;
        }
    }

    private void Update()
    {
        if (_grass != null)
        {
            var count = _grass.RemoveInRadius(transform.position, _sphereCollider.radius);
            OnCollideGrass?.Invoke(count);
            _mowedCount += count;
            if (_mowedCount >= _grassInOneBlock)
            {
                _mowedCount -= _grassInOneBlock;
                Instantiate(_grassBlock, transform.position, Quaternion.identity);
            }
        }
    }

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }
}
