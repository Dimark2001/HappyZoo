using UnityEngine;

[RequireComponent(typeof(Mower))]
public class MowerView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private MeshRenderer[] _renderers;
    [SerializeField] private float _rotationSpeed = 720f;
    [SerializeField] private IKControl _ik;
    
    private Mower _mower;
    
    private void Start()
    {
        _mower = GetComponent<Mower>();
        
        _mower.OnCollideGrass += OnCollideGrass;
        _mower.OnActivate += OnActiveStateChanged;
        _mower.OnDeactivate += OnActiveStateChanged;
        
        SetMowerViewActive(false);
    }

    private void OnDestroy()
    {
        _mower.OnCollideGrass -= OnCollideGrass;
        _mower.OnActivate -= OnActiveStateChanged;
        _mower.OnDeactivate -= OnActiveStateChanged;
    }

    private void OnActiveStateChanged()
    {
        SetMowerViewActive(_mower.IsActive);
    }

    private void OnCollideGrass(int count)
    {
        _particles.gameObject.SetActive(true);
        _particles.emission.SetBurst(0, new ParticleSystem.Burst(0, count));
        _particles.Play();
    }

    private void SetMowerViewActive(bool isActive)
    {
        foreach (var meshRenderer in _renderers)
        {
            meshRenderer.enabled = isActive;
            _ik.ikActive = isActive;
        }
    }
    
    private void Update()
    {
        if (_mower.IsNotActive) return;
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + Vector3.up * _rotationSpeed * Time.deltaTime);
    }
}
