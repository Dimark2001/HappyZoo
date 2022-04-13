using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlayerControls))]
public class PlayerMovement : MonoBehaviour
{
    public bool CanMoveOnlyX = false;

    public float CurrentSpeed
    {
        get => currentSpeed;
        set
        {
            if (value < 0)
                currentSpeed = 0;
            currentSpeed = value;
            OnSpeedChanged?.Invoke();
        }
    }
    public Action OnSpeedChanged;
    public float MaxSpeed = 8f;
    
    [SerializeField] private float _acceleration = 10f;
    
    private PlayerControls _playerControls;
    private Collider _collider;
    private Rigidbody _body;
    private Transform _transform;
    private Vector3  _lookDirection = Vector3.forward;
    private float currentSpeed;
    private Vector3 _lastMoveDirection = Vector3.zero;

    private void Start()
    {
        _playerControls = GetComponent<PlayerControls>();
        _collider = GetComponent<Collider>();
        _body = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
    }
    
    void FixedUpdate()
    {
        Vector3 moveDirection = _playerControls.MoveDirection;
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
        
        _lookDirection = moveDirection == Vector3.zero ? _lookDirection : moveDirection;
        _transform.forward = _lookDirection;

        if (CanMoveOnlyX)
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z/6);

        if (moveDirection.magnitude > 0.05f)
        {
            CurrentSpeed = Math.Min(CurrentSpeed + _acceleration * Time.fixedDeltaTime, MaxSpeed * _playerControls.SpeedMultiplier);
        }
        else
        {
            CurrentSpeed = Math.Max(CurrentSpeed - _acceleration * Time.fixedDeltaTime, 0);
        }

        _lastMoveDirection = moveDirection == Vector3.zero ? _lastMoveDirection : moveDirection;
        Vector3 movePosition = _lastMoveDirection * CurrentSpeed * Time.fixedDeltaTime;

        _body.velocity = movePosition;



    }
}
