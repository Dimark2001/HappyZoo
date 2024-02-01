using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class NewGrass : MonoBehaviour
{
    [SerializeField] private GameObject grass;
    [SerializeField] private Transform grassParent;
    [SerializeField] private List<GameObject> grassList;

    private void Start()
    {
        SpawnGrass();
    }

    public void ResetGrass()
    {
        DeleteGrass();
        SpawnGrass();
    }

    [Button()]
    private void SpawnGrass()
    {
        grassList ??= new List<GameObject>();
        for (var i = 0; i < 9; i++)
        {
            for (var j = -9; j <= 9; j++)
            {
                var rand = Random.Range(0, 360);
                var newAngle = new Vector3(0, rand, 0);
                var item = Instantiate(grass, Vector3.zero, Quaternion.Euler(newAngle), grassParent);
                grassList.Add(item);
                var pos = new Vector3(j, 0, 4.25f - i*1);
                item.transform.localPosition = pos;
            }
        }
    }
    
    [Button()]
    private void DeleteGrass()
    {
        grassList ??= new List<GameObject>();
        for (var i = grassList.Count - 1; i >= 0; i--)
        {
            var o = grassList[i];
            DestroyImmediate(o);
        }
        grassList.Clear();
    }
}
