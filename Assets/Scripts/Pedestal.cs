using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

[RequireComponent(typeof(Collider))]
public class Pedestal : MonoBehaviour
{
    [SerializeField] private Resource _resource;
    [SerializeField] private float _givingTime = 1f;
    [SerializeField] private Transform _origin;
    [SerializeField] private bool _oneShot;
    [SerializeField] private MeshRenderer _renderer;

    private TweenerCore<Vector3, Vector3, VectorOptions> _tweenerCore = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ResourcesStack resourcesStack))
        {
            if ((_tweenerCore == null || !_tweenerCore.active) && !resourcesStack.IsFull)
            {
                SpawnAndMoveResource(resourcesStack);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ResourcesStack resourcesStack))
        {
            if (_tweenerCore == null || _tweenerCore.active)
            {
                _tweenerCore.Kill();
            }
        }
    }

    private void SpawnAndMoveResource(ResourcesStack resourcesStack)
    {
        Resource res = Instantiate(_resource);
        res.transform.position = _origin.position;

        var position = resourcesStack.GetNextItemPosition(res);
                
        var scaling = res.transform.DOScale(Vector3.one * 0.8f, _givingTime);
        var rotating = res.transform.DORotateQuaternion(resourcesStack.transform.rotation, _givingTime);

        if (_oneShot)
        {
            _renderer.enabled = false;
            _tweenerCore = res.transform.DOMove(position, _givingTime / 3);
            _tweenerCore
                .OnComplete(() =>
                {
                    resourcesStack.AddResource(_resource);
                    if (resourcesStack.IsFull)
                    {
                        _renderer.enabled = true;
                        _tweenerCore.Kill();
                    }
                })
                .OnKill(() =>
                {
                    scaling.Kill();
                    rotating.Kill();
                    Destroy(res.gameObject);
                    Destroy(gameObject);
                });
            _tweenerCore.SetEase(Ease.InQuad);
        }
        else
        {
             _tweenerCore = res.transform.DOMove(position, _givingTime);
            _tweenerCore
                .OnComplete(() =>
                {
                    resourcesStack.AddResource(_resource);
                    if (resourcesStack.IsFull)
                    {
                        _tweenerCore.Kill();
                        return;
                    }
                    SpawnAndMoveResource(resourcesStack);
                })
                .OnKill(() =>
                {
                    scaling.Kill();
                    rotating.Kill();
                    Destroy(res.gameObject);
                });
            _tweenerCore.SetEase(Ease.InQuad);
        }
    }
}
