using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Animator))]
public class Pig : MonoBehaviour
{
    public PigFarm pigFarm;

    [SerializeField] private float _waitTime = 3f;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private GameObject _meatPrefab;
    [SerializeField] private GameObject _particlesPrefab;

    private bool _isStanding;
    private Animator _animator;

    public Action<Pig> OnDeath;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            var position = transform.position;
            
            StopCoroutine(WalkingLoopCoroutine());
            DOTween.Kill(this);

            GetComponent<Collider>().enabled = false;
            var deathPosition = position + (position - playerMovement.transform.position).normalized * 2 + Vector3.up * 3;
            transform.DOMove(deathPosition, 0.35f).SetEase(Ease.OutBack);
            transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                Instantiate(_meatPrefab, deathPosition, Quaternion.identity);
                Instantiate(_particlesPrefab, deathPosition + Vector3.up * 2f, Quaternion.identity);
                OnDeath?.Invoke(this);
                Destroy(gameObject);
            }).SetEase(Ease.InBack);
        }
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        
        StartCoroutine(WalkingLoopCoroutine());
    }

    private IEnumerator WalkingLoopCoroutine()
    {
        while (true)
        {
            var position = transform.position;
            var point = pigFarm.GetPoint();
            
            var distance = Vector3.Distance(position, point);
            var walkTime = distance / _speed;

            transform.DOLookAt(point, 0.5f, AxisConstraint.X | AxisConstraint.Z);
            transform.DOMove(point, walkTime).SetEase(Ease.Linear).OnComplete(() => _isStanding = true);
            _isStanding = false;
            _animator.SetTrigger("Walk");
            
            yield return new WaitForSeconds(walkTime);
            
            
            _isStanding = true;
            
            _animator.SetTrigger("Idle");
            yield return new WaitForSeconds(_waitTime);
        }
    }
}
