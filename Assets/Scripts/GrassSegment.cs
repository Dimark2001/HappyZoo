using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider))]
public class GrassSegment : MonoBehaviour
{
    [SerializeField] private float _regenerationTime = 15f;
    [SerializeField] private GameObject _grassStack;
    
    private Collider _collider;
    
    void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Mower mower))
        {
            //mower.PlayParticles();
            Mow();
            if (Random.Range(0f, 1f) < 0.5f)
            {
                var stack = Instantiate(_grassStack, transform.position, Quaternion.identity);
                Destroy(stack, 10);
            }

            StartCoroutine(RegeneratingCoroutine());
        }
    }

    private void Mow()
    {
        _collider.enabled = false;
    }

    private IEnumerator RegeneratingCoroutine()
    {
        yield return new WaitForSeconds(_regenerationTime);
        _collider.enabled = true;
    }
}
