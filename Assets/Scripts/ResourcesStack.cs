using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesStack : MonoBehaviour
{
    public List<Resource> Resources = new List<Resource>();
    public Action OnStackChanged;

    [SerializeField] private Transform _resourceStackOrigin;

    public void AddResource(Resource resource)
    {
        Resource res = Instantiate(resource);
        // TODO: въебенить анимацию
        Resources.Add(res);
        RecalculateHeight();
        OnStackChanged?.Invoke();
    }

    public bool TryRemoveResource(Resource resource)
    {
        foreach (Resource res in Resources)
        {
            if (res.Prefab == resource.Prefab)
            {
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
