using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Vector2 MoveDirection => _isMoving ? -(TouchStart-TouchEnd).normalized : Vector2.zero;
    public float SpeedMultiplier => Mathf.Min(Vector2.Distance(TouchStart, TouchEnd)/_maxStickRadius, 1);
    
    public Vector2 TouchPosition => _defaultControls.Default.TouchPosition.ReadValue<Vector2>();
    
    public Vector2 TouchStart;
    public Vector2 TouchEnd => TouchPosition;

    public float TouchDistance => Vector2.Distance(TouchStart, TouchEnd);
    
    [SerializeField] private float _maxStickRadius = 150;
    
    private DefaultControls _defaultControls;
    private bool _isMoving = false;

    void Awake()
    {
        _defaultControls = new DefaultControls();
    }

    void OnEnable()
    {
        _defaultControls.Enable();
        _defaultControls.Default.Touch.started += (ctx) => OnTouchStarted();
        _defaultControls.Default.Touch.canceled += (ctx) => OnTouchCanceled();
    }
    
    void OnDisable()
    {
        _defaultControls.Disable();
    }

    private void OnTouchStarted()
    {
        _isMoving = true;
        TouchStart = TouchPosition;
    }
    
    private void OnTouchCanceled()
    {
        _isMoving = false;
        TouchStart = TouchPosition;
    }

    private void Update()
    {
        if (TouchDistance > _maxStickRadius && _isMoving)
        {
            float error = (TouchDistance - _maxStickRadius)/_maxStickRadius;
            TouchStart = Vector2.Lerp(TouchStart, TouchEnd, error);
        }
    }
}
