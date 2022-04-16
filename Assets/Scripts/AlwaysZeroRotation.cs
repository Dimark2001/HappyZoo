using UnityEngine;

public class AlwaysZeroRotation : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.identity;
        
    }
}
