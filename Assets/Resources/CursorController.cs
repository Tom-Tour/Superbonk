using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    // REFERENCES
    private Camera camera;
    private Cursor cursor;
    private PlayerInput playerInput;
    
    // INFORMATIVES
    private Vector2 direction = Vector2.zero;
    private bool isUsingMouseAndKeyboard = false;
    private bool isReady = false;
    private bool isLocal = true;
    private Vector2 lastMousePosition;

    private void Awake()
    {
        camera = Camera.main;
        cursor = GetComponent<Cursor>();
        playerInput = GetComponent<PlayerInput>();
        isUsingMouseAndKeyboard = playerInput.currentControlScheme == "Keyboard&Mouse";
    }
    private void Update()
    {
        if (isUsingMouseAndKeyboard)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            if (lastMousePosition != mouseScreenPos)
            {
                Vector3 mouseWorldPos = camera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, camera.nearClipPlane));
                mouseWorldPos.z = 0;
                cursor.Teleport(mouseWorldPos);
                lastMousePosition = mouseScreenPos;
            }
        }
    }
    private void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>().normalized;
        cursor.SetDirection(direction);
    }
    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            cursor.Ready();
        }
    }
    private void OnFire(InputValue value)
    {
        if (value.isPressed)
        {
            cursor.Ready();
        }
    }
}
