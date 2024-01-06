using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider))]
public class PigFarm : MonoBehaviour
{
    [SerializeField] private Pig _pigPrefab;
    [SerializeField] private float _spawningDelay = 2f;
    [SerializeField] private int _maxPigs = 6;
    
    private BoxCollider _box;
    private int _pigsCount;
    
    public Vector3 GetPoint()
    {
        if (_box != null)
        {
            var boxSize = _box.size;
            var boxCenter = _box.center;
            var width = boxSize.x;
            var depth = boxSize.z;

            var x = Random.Range(-width / 2, width / 2);
            var z = Random.Range(-depth / 2, depth / 2);

            var localPoint = new Vector3(boxCenter.x + x, 0, boxCenter.z + z);
            var globalPoint = transform.TransformPoint(localPoint);
        
            return globalPoint;
        }
        
        return Vector3.zero;
    }

    private void SpawnPig()
    {
        var pig = Instantiate(_pigPrefab, GetPoint(), Quaternion.identity);
        pig.transform.localScale = Vector3.zero;
        pig.transform.position += Vector3.up;
        
        pig.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        pig.transform.DOMoveY(transform.position.y - 1, 0.5f);
        pig.pigFarm = this;
        pig.OnDeath += OnPigDeath;
        _pigsCount++;
    }

    private void OnPigDeath(Pig pig)
    {
        pig.OnDeath -= OnPigDeath;
        _pigsCount--;
    }

    private void Awake()
    {
        _box = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        StartCoroutine(SpawningLoopCoroutine());
    }

    private IEnumerator SpawningLoopCoroutine()
    {
        while (true)
        {
            if (_pigsCount < _maxPigs)
            {
                SpawnPig();
            }

            yield return new WaitForSeconds(_spawningDelay);
        }
    }
}
