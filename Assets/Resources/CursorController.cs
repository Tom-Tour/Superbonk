using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class CursorController : NetworkBehaviour
{
    // REFERENCES
    private PlayerInput playerInput;
    private Camera camera;
    private NetworkObject networkObject;
    
    // MODIFIERS
    private float speed = 4;
    
    // INFORMATIVES
    private Vector2 direction = Vector2.zero;
    private bool isUsingMouseAndKeyboard = false;
    private bool isReady = false;
    private bool isLocal = true;
    private Vector2 lastMousePosition;

    private void Awake()
    {
        camera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        isUsingMouseAndKeyboard = playerInput.currentControlScheme == "Keyboard&Mouse";
    }
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            isLocal = false;
            playerInput.enabled = false;
        }
    }
    public override void OnNetworkDespawn()
    {
        isLocal = true;
        playerInput.enabled = true;
    }
    private void Update()
    {
        if (isLocal)
        {
            transform.Translate(direction * (speed * Time.deltaTime));
            if (isUsingMouseAndKeyboard)
            {
                Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
                if (lastMousePosition != mouseScreenPos)
                {
                    Vector3 mouseWorldPos = camera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, camera.nearClipPlane));
                    mouseWorldPos.z = 0;
                    transform.position = mouseWorldPos;
                    lastMousePosition = mouseScreenPos;
                }
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
    private void OnFire(InputValue value)
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
