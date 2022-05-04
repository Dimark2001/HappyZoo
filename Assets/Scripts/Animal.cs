using System.Collections;
using GameAnalyticsSDK.Setup;
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

    [SerializeField] private float _minPoopingTime = 20f;
    [SerializeField] private float _maxPoopingTime = 40f;
    [SerializeField] private Transform _assHole;
    [SerializeField] private GameObject _poop;
    private float _poopingTime => Random.Range(_minPoopingTime, _maxPoopingTime);
    
    private float _waitTime => Random.Range(_minWaitTime, _maxWaitTime);
    private NavMeshAgent _agent;
    private bool _hungry = false;
    private bool _notHungry => !_hungry;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        StartCoroutine(WalkingLoop());
        StartCoroutine(PoopingLoop());
        
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
        if (!GoTo(_homePoint.position))
        {
            transform.position = _homePoint.position;
        }
        _hungry = true;
    }

    private void GoOut()
    {
        var position = _paddock.paddockStack.PlaceToStand;
        if (!GoTo(position))
        {
            transform.position = position;
        }
    }

    private void BeNotHungry() => _hungry = false;

    private bool GoTo(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        bool result = _agent.CalculatePath(position, path);
        bool result1 = _agent.SetPath(path);

        return result && result1;
    }
    
    private IEnumerator PoopingLoop()
    {
        yield return new WaitForSeconds(_poopingTime);
        if (_notHungry)
        {
            var poo = Instantiate(_poop, _assHole.position, Quaternion.identity);
            var direction = _assHole.up + (_assHole.right * Random.Range(-0.2f, 0.2f)) + (_assHole.forward * Random.Range(-0.2f, 0.2f)) ;
            poo.GetComponent<Rigidbody>().AddForce(direction * Random.Range(3, 10)  , ForceMode.Impulse);
        }

        StartCoroutine(PoopingLoop());
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
