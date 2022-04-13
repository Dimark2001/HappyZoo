using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider))]
public class PointGiver : MonoBehaviour
{
    private BoxCollider _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    public Vector3 GetPoint()
    {
        var min = _boxCollider.bounds.min;
        var max = _boxCollider.bounds.max;

        var x = Random.Range(min.x, max.x);
        var y = Random.Range(min.y, max.y);
        var z = Random.Range(min.z, max.z);

        return new Vector3(x, y, z);
    }
}
