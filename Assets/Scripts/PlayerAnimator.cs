using UnityEngine;


[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private PlayerMovement _playerMovement;
    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        _playerMovement.OnSpeedChanged += OnSpeedChanged;
    }

    private void OnDisable()
    {
        _playerMovement.OnSpeedChanged -= OnSpeedChanged;
    }

    void OnSpeedChanged()
    {
        float normalizedSpeed = _playerMovement.CurrentSpeed / _playerMovement.MaxSpeed;
        
        _animator.SetFloat("Speed", normalizedSpeed);
    }
}
