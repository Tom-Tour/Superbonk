using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class Cursor : NetworkBehaviour
{
    // REFERENCES
    private PlayerInput playerInput;
    private PlayerIdentifier playerIdentifier;
    
    // MODIFIERS
    private float speed = 4;
    
    // INFORMATIVES
    private Vector2 direction = Vector2.zero;
    private CharacterInformation characterInformation;
    private bool isReady = false;
    private bool isLocal = true;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            playerInput.enabled = false;
            isLocal = false;
        }
    }
    public override void OnNetworkDespawn()
    {
        playerInput.enabled = true;
        isLocal = true;
    }
    
    private void Update()
    {
        if (isLocal)
        {
            transform.Translate(direction * (speed * Time.deltaTime));
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isLocal)
        {
            CharacterInformation currentCharacterInformation = other.GetComponent<CharacterInformation>();
            if (currentCharacterInformation != null)
            {
                characterInformation = currentCharacterInformation;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isLocal)
        {
            CharacterInformation currentCharacterInformation = other.GetComponent<CharacterInformation>();
            if (currentCharacterInformation == characterInformation)
            {
                characterInformation = null;
            }
        }
    }

    public void Teleport(Vector3 position)
    {
        if (isLocal)
        {
            transform.position = position;
        }
    }
    public void SetDirection(Vector2 newDirection)
    {
        if (isLocal)
        {
            direction = newDirection;
        }
    }

    public void Ready()
    {
        if (isLocal)
        {
            if (characterInformation != null)
            {
                isReady = !isReady;
                if (isReady)
                {
                    UpdateCharacterInformationServerRpc(characterInformation.characterId);
                }
            }
        }
    }

    [ServerRpc]
    private void UpdateCharacterInformationServerRpc(int characterId)
    {
        playerIdentifier.SetCharacterId(characterId);
    }
}
