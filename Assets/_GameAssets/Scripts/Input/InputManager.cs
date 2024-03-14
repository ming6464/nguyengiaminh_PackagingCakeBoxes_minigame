using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
#region Event

    public delegate void StartTouch(Vector2 position, float time);

    public delegate void EndTouch(Vector2 position, float time);

    public event StartTouch OnStartTouch;
    public event EndTouch OnEndTouch;

#endregion

    private CharacterInput m_characterInput;
    private Camera m_camera;

    public override void Awake()
    {
        m_characterInput = new CharacterInput();
        m_camera = Camera.main;
    }

    private void OnEnable()
    {
        m_characterInput.Enable();
    }

    private void OnDisable()
    {
        m_characterInput.Disable();
    }

    private void Start()
    {
        m_characterInput.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        m_characterInput.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    private void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        if(OnEndTouch == null) return;
        OnEndTouch(GetPositionPrimary(), (float)ctx.time);
    }

    private void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        if(OnStartTouch == null) return;
        OnStartTouch(GetPositionPrimary(), (float)ctx.startTime);
    }

    private Vector2 GetPositionPrimary()
    {
        return ScreenToWorld(m_characterInput.Touch.PrimaryPosition.ReadValue<Vector2>());
    }

    private Vector3 ScreenToWorld(Vector3 position)
    {
        if (!m_camera) return Vector3.zero;
        position.z = m_camera.nearClipPlane;
        return m_camera.ScreenToWorldPoint(position);
    }
    
}