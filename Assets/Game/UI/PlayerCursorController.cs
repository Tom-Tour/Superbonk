using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class PlayerCursorController : NetworkBehaviour
{
    // REFERENCES
    private PlayerInput playerInput;
    private NetworkObject networkObject;

    // MODIFIERS
    private float speed = 4;
    
    // INFORMATIVES
    private bool initialized = false;
    [HideInInspector] public bool isLocal = true;
    private Vector2 direction = Vector2.zero;
    private bool isReady = false;

    private void OnEnable()
    {
        if (initialized)
        {
            Register();
        }
    }
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        networkObject = GetComponent<NetworkObject>();
    }
    private void Start()
    {
        if (isLocal)
        {
            networkObject.enabled = false;
        }
        initialized = true;
        Register();
    }
    private void Update()
    {
        if (!IsOwner && NetworkManager.Singleton.IsListening)
        {
            return;
        }
        transform.Translate(direction * (speed * Time.deltaTime));
        // transform.position += direction * (speed * Time.deltaTime);
    }
    public override void OnDestroy()
    {
        Unregister();
        base.OnDestroy();
    }
    private void OnDisable()
    {
        Unregister();
    }
    private void OnMove(InputValue value)
    {
        if (!IsOwner && NetworkManager.Singleton.IsListening)
        {
            return;
        }
        direction = value.Get<Vector2>().normalized;
        isReady = false;
    }
    private void OnJump(InputValue value)
    {
        if (!IsOwner && NetworkManager.Singleton.IsListening)
        {
            return;
        }
        if (value.isPressed)
        {
            isReady = !isReady;
        }
    }
    private void Register()
    {
        if (isLocal)
        {
            InputHandler_.Instance.RegisterLocalPlayerInput(playerInput);
        }
        else
        {
            InputHandler_.Instance.RegisterNetworkPlayerInput(playerInput);
        }
    }
    private void Unregister()
    {
        if (isLocal)
        {
            InputHandler_.Instance.UnregisterLocalPlayerInput(playerInput);
        }
        else
        {
            InputHandler_.Instance.UnregisterNetworkPlayerInput(playerInput);
        }
    }
}