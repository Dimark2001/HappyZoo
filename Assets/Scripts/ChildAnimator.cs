using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ChildAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private NavMeshAgent _agent;
    private Customer _customer;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _customer = GetComponent<Customer>();
    }

    private void Update()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude);

        if (_customer.IsLooking && _agent.velocity.magnitude == 0)
        {
            if (Random.Range(0, 2) == 1)
            {
                _animator.SetTrigger("Cheering");
                _customer.IsLooking = false;
            }
        }
    }
}
