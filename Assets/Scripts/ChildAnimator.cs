using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ChildAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude); 
    }
}
