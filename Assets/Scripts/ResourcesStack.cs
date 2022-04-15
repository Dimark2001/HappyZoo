using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesStack : MonoBehaviour
{
    public List<Resource> Resources = new List<Resource>();
    public Action OnStackChanged;
    public bool IsFull => Resources.Count >= _maxCount;

    [SerializeField] private int _maxCount = 12;
    [SerializeField] private Transform _resourceStackOrigin;
    [SerializeField] private GameObject _maxIndicator;

    private long[] pattern = { 0, 10, 20, 20};

    private void Start()
    {
        Vibration.Init();
    }

    public void AddResource(Resource resource)
    {
        if (Resources.Count < _maxCount)
        {
            Resource res = Instantiate(resource);
            Resources.Add(res);
            RecalculateHeight();
            OnStackChanged?.Invoke();
            Vibration.Vibrate(pattern, -1);
        }
        if (Resources.Count == _maxCount)
        {
            var height = Resources.Count == 0 ? 0 : Resources.Max(x => x.CurrentHeight) + 1f;
            var position = _resourceStackOrigin.transform.position + Vector3.up * height;

            _maxIndicator.transform.position = position;
            _maxIndicator.SetActive(true);
        }
    }

    public bool TryRemoveResource(Resource resource)
    {
        foreach (Resource res in Resources)
        {
            if (res.Prefab == resource.Prefab)
            {
                _maxIndicator.SetActive(false);
                Destroy(res.gameObject);
                Resources.Remove(res);

                RecalculateHeight();
                OnStackChanged?.Invoke();

                return true;
            }
        }
        return false;
    }

    private void RecalculateHeight()
    {
        float previousHeight = 0;
        float previousCurrentHeight = 0;
        ResetHeight();
        
        for (int i = 0; i < Resources.Count; i++)
        {
            Resources[i].CurrentHeight = previousCurrentHeight + previousHeight/2 + Resources[i].Height;
            
            previousCurrentHeight = Resources[i].CurrentHeight;
            previousHeight = Resources[i].Height;
        }
    }

    public Vector3 GetNextItemPosition(Resource resource)
    {
        var height = Resources.Count == 0 ? 0 : Resources.Max(x => x.CurrentHeight) + resource.Height;
        var position = _resourceStackOrigin.transform.position + Vector3.up * height;
        return position;
    }

    private void ResetHeight()
    {
        for (int i = 0; i < Resources.Count; i++)
        {
            Resources[i].CurrentHeight = 0;
        }
    }
    
}
