using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class Animal : MonoBehaviour
{
    [SerializeField] private PointGiver _pointGiver;
    [SerializeField] private Paddock _paddock;
    [SerializeField] private Transform _homePoint;
    [SerializeField] private float _minWaitTime = 1f;
    [SerializeField] private float _maxWaitTime = 3f;
    [SerializeField] private Animator _animator;
    private float _waitTime => Random.Range(_minWaitTime, _maxWaitTime);
    private NavMeshAgent _agent;
    private bool _hungry = false;
    private bool _notHungry => !_hungry;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        StartCoroutine(WalkingLoop());
        
        _paddock.OnGetHungry += GoHome;
        _paddock.OnAte += GoOut;
        _paddock.paddockStack.Emptied += BeNotHungry;
    }

    private void OnDisable()
    {
        _paddock.OnGetHungry -= GoHome;
        _paddock.OnAte -= GoOut;
        _paddock.paddockStack.Emptied -= BeNotHungry;
    }

    private void Update()
    {
        var speed = _agent.velocity.magnitude;
        _animator.SetFloat("Speed", speed);
    }

    private void GoHome()
    {
        GoTo(_homePoint.position);
        _hungry = true;
    }

    private void GoOut()
    {
        var position = _paddock.paddockStack.PlaceToStand;
        GoTo(position);
        Debug.Log("Выходят наружу");
    }

    private void BeNotHungry() => _hungry = false;

    private void GoTo(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(position, path);
        _agent.SetPath(path);
    }
    
    private IEnumerator WalkingLoop()
    {
        yield return new WaitForSeconds(_waitTime);
        if (_notHungry)
        {
            var position = _pointGiver.GetPoint();
            GoTo(position);
        }

        StartCoroutine(WalkingLoop());
    }
}
