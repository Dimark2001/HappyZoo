using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Trashcan : MonoBehaviour
{
    [SerializeField] private float _takingTime = 0.5f;
    [SerializeField] private Transform _foodTakerPosition;
    private TweenerCore<Vector3, Vector3, VectorOptions> _tweenerCore;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ResourcesStack resourcesStack))
        {
            if (resourcesStack.Resources.Count > 0)
            {
                TakeFood(resourcesStack);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ResourcesStack resourcesStack))
        {
            _tweenerCore.Kill();
        }
    }

    private void TakeFood(ResourcesStack resourcesStack)
    {
        Resource resource = resourcesStack.Resources[resourcesStack.Resources.Count - 1];
        if (resource == null)
            return;
        
        Resource res = Instantiate(resource);
        
        res.transform.position = resource.transform.position;

        var position = _foodTakerPosition.position;
        
        var rotating = res.transform.DORotateQuaternion(resourcesStack.transform.rotation, _takingTime);

        _tweenerCore = res.transform.DOMove(position, _takingTime);
        _tweenerCore
            .OnComplete(() =>
            {
                bool result = resourcesStack.TryRemoveResource(res);
                if (result)
                {
                    if (resourcesStack.Resources.Count == 0)
                    {
                        _tweenerCore.Kill();
                        return;
                    }

                    TakeFood(resourcesStack);
                }
            })
            .OnKill(() =>
            {
                rotating.Kill();
                Destroy(res.gameObject);
            });
        _tweenerCore.SetEase(Ease.InQuad);
    }
}
