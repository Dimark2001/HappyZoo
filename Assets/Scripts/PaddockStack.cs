using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PaddockStack : MonoBehaviour
{
    public Action Emptied;
    public Action OnFoodRemoved;
    public Vector3 PlaceToStand => origin.position + new Vector3(Random.Range(-4f, 4f),0, Random.Range(-4f, 4f));
    public Transform origin;
    public int neededFood;
    
    private List<Resource> _resources = new List<Resource>();
    private long[] pattern = { 0, 10, 20, 20};

    public void AddResource(Resource resource)
    {
        var res = Instantiate(resource);
        _resources.Add(res);
        UpdateView();
    }

    private void RemoveOneResource()
    {
        if (neededFood > 0)
        {
            Destroy(_resources[0].gameObject);
            _resources.RemoveAt(0);
            OnFoodRemoved?.Invoke();
            neededFood--;
        }

        if (neededFood <= 0)
        {
            Emptied?.Invoke();
        }

        UpdateView();
    }

    private void UpdateView()
    {
        RecalculateHeight();
        foreach (var resource in _resources)
        {
            resource.transform.parent = origin;
            resource.transform.localPosition = Vector3.up * resource.CurrentHeight;
            resource.transform.localRotation = Quaternion.identity;
            resource.transform.localScale = Vector3.one * 0.8f;
        }
    }
    
    private void RecalculateHeight()
    {
        float previousHeight = 0;
        float previousCurrentHeight = 0;
        ResetHeight();
        
        foreach (var t in _resources)
        {
            t.CurrentHeight = previousCurrentHeight + previousHeight/2 + t.Height;
            
            previousCurrentHeight = t.CurrentHeight;
            previousHeight = t.Height;
        }
    }
    
    private void ResetHeight()
    {
        foreach (var t in _resources)
        {
            t.CurrentHeight = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Animal animal))
        {
            StartCoroutine(Eat());
        }
    }

    private IEnumerator Eat()
    {
        yield return new WaitForSeconds(1);
        RemoveOneResource();
        if (neededFood <= 0)
        {
            StopCoroutine(Eat());
            yield break;
        }

        
        StartCoroutine(Eat());
    }
    
}
