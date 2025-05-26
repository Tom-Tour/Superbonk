using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor : NetworkBehaviour
{
    // COMPONENTS
    private SpriteRenderer spriteRenderer;
    
    // INFORMATIVES
    public bool isReady { get; private set; } = false;
    private Color color;
    private Vector3 direction;
    
    // MODIFIERS
    private float speed = 12;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // StartCoroutine(Login());
        if (IsServer)
        {
            // GetComponent<NetworkObject>().Spawn();
            // GetComponent<NetworkObject>().SpawnAsPlayerObject(GetComponent<PlayerInput>().user.id);
        }
    }

    private IEnumerator Login()
    {
        while (!(NetworkManager.Singleton.IsClient && NetworkManager.Singleton.IsConnectedClient))
        {
            Debug.Log("Connecting..");
            yield return new WaitForSeconds(4f);
        }
        if (!IsSpawned)
        {
            // SelectionCharacter.Instance.RequestSpawnPlayerCursorServerRpc(this);
        }
        Debug.Log("Connected !");
    }


    private void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
    }
    
    void OnMove(InputValue value)
    {
        if (NetworkManager.Singleton.IsListening && !IsOwner)
        {
            return;
        }
        direction = value.Get<Vector2>().normalized;
        isReady = false;
    }

    void OnJump(InputValue value)
    {
        if (NetworkManager.Singleton.IsListening && !IsOwner)
        {
            return;
        }
        if (value.isPressed)
        {
            isReady = !isReady;
        }
    }

    public void ChangeColor(int colorID)
    {
        spriteRenderer.color = Palette.rainbowColors[colorID];
    }
}
