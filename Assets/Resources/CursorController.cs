using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class CursorController : NetworkBehaviour
{
    // REFERENCES
    private PlayerInput playerInput;
    
    // MODIFIERS
    private float speed = 4;
    
    // INFORMATIVES
    private Vector2 direction = Vector2.zero;
    private bool IsUsingMouseAndKeyboard = false;
    private bool isReady = false;
    private bool isLocal = true;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        IsUsingMouseAndKeyboard = playerInput.currentControlScheme == "Keyboard&Mouse";
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            isLocal = false;
            playerInput.enabled = false;
        }
    }
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        isLocal = true;
        playerInput.enabled = true;
    }
    private void Update()
    {
        if (isLocal)
        {
            transform.Translate(direction * (speed * Time.deltaTime));
            if (IsUsingMouseAndKeyboard)
            {
                // TODO : Check mouse last position, if last position different than actual position then move mouse.
                /*
                Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
                mouseWorldPos.z = 0;

                transform.position = Vector3.Lerp(transform.position, mouseWorldPos, Time.deltaTime * speed);
                */
            }
        }
    }
    private void OnMove(InputValue value)
    {
        if (isLocal)
        {
            direction = value.Get<Vector2>().normalized;
            isReady = false;
        }
    }
    private void OnJump(InputValue value)
    {
        if (isLocal)
        {
            if (value.isPressed)
            {
                isReady = !isReady;
            }
        }
    }
}
