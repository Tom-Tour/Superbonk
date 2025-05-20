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
        color = Palette.rainbowColors[Random.Range(0, Palette.rainbowColors.Length)];
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }

    private void Start()
    {
        StartCoroutine(Login());
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
            SelectionCharacter.Instance.RequestSpawnPlayerCursorRpc(this);
        }
        Debug.Log("Connected !");
    }


    private void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
    }
    
    void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>().normalized;
        isReady = false;
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            isReady = !isReady;
        }
    }
}
