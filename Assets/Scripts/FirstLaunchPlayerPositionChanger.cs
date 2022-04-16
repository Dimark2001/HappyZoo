using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLaunchPlayerPositionChanger : MonoBehaviour
{
    [SerializeField] private Transform _position;
    
    void Start()
    {
        if (PlayerPrefs.GetInt("IsFirstLaunch", 1) == 1)
        {
            transform.position = _position.position;
            PlayerPrefs.SetInt("IsFirstLaunch", 0);
            PlayerPrefs.Save();
        }
    }
}
