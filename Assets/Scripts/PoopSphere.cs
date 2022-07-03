using System.Linq;
using UnityEngine;

public class PoopSphere : MonoBehaviour
{
    [SerializeField] private GameObject _poop;
    [SerializeField] private int _hp = 3;
    [SerializeField] private float _lastCollisionTime;

    private MeshRenderer _renderer;
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();
        _lastCollisionTime = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad - _lastCollisionTime > 5)
        {
            Instantiate(_poop, transform.position, Quaternion.identity);
            _renderer.enabled = false;
            _collider.enabled = false;
            Destroy(gameObject, 1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Any(col => Vector3.Dot(col.normal, Vector3.up) >= 0.9f))
        {
             _lastCollisionTime = Time.timeSinceLevelLoad;
            _hp--;
            if (_hp <= 0)
            {
                Instantiate(_poop, transform.position, Quaternion.identity);
                _renderer.enabled = false;
                _collider.enabled = false;
                Destroy(gameObject, 1);
            }
        }
    }
    
    
}
