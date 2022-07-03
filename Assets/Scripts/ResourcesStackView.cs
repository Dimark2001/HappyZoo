using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourcesStack))]
public class ResourcesStackView : MonoBehaviour
{
    [SerializeField] private Transform origin;

    private ResourcesStack _resourcesStack;
    private List<GameObject> _models = new List<GameObject>();

    private void Awake()
    {
        _resourcesStack = GetComponent<ResourcesStack>();
    }

    private void OnEnable()
    {
        _resourcesStack.OnStackChanged += UpdateView;
    }

    private void OnDisable()
    {
        _resourcesStack.OnStackChanged -= UpdateView;
    }

    private void UpdateView()
    {

        foreach (var resource in _resourcesStack.Resources)
        {
            resource.transform.parent = origin;
            resource.transform.localPosition = Vector3.up * resource.CurrentHeight;
            resource.transform.localRotation = Quaternion.identity;
            resource.transform.localScale = Vector3.one * 0.8f;
        }
    }


}
