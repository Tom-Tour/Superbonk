using System;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class Cursor : NetworkBehaviour
{
    // REFERENCES
    private PlayerInput playerInput;
    public PlayerIdentifier playerIdentifier { get; private set; }
    
    // MODIFIERS
    private float speed = 4;
    
    // INFORMATIVES
    private Vector2 direction = Vector2.zero;
    private CharacterInformation hoverCharacterInformation;
    private CharacterInformation selectedCharacterInformation;
    private bool isReady = false;
    private bool isLocal = true;
    private int playerIndex;
    
    public static event Action<int, bool> OnPlayerReady;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerIdentifier = GetComponent<PlayerIdentifier>();
    }
    private void Start()
    {
        playerIndex = InputHandler.Instance.PlayerIndexes.Count - 1;
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
                hoverCharacterInformation = currentCharacterInformation;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isLocal)
        {
            CharacterInformation currentCharacterInformation = other.GetComponent<CharacterInformation>();
            if (currentCharacterInformation == hoverCharacterInformation)
            {
                hoverCharacterInformation = null;
            }
        }
    }

    public void ForceStart()
    {
        if (!NetworkManager.Singleton.IsListening)
        {
            StageManager.Instance.LoadScene("Arena");
        }
        else if (IsServer)
        {
            StageManager.Instance.LoadSceneNetwork("Arena");
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
            if (hoverCharacterInformation != null)
            {
                if (selectedCharacterInformation == hoverCharacterInformation)
                {
                    isReady = !isReady;
                    selectedCharacterInformation = null;
                }
                else
                {
                    isReady = true;
                    selectedCharacterInformation = hoverCharacterInformation;
                }
                if (NetworkManager.Singleton.IsListening)
                {
                    ReadyServerRpc(isReady);
                }
                else
                {
                    OnPlayerReady?.Invoke(playerIndex, isReady);
                }
            }
        }
    }

    [ServerRpc]
    private void ReadyServerRpc(bool isReadyFromClient, ServerRpcParams rpcParams = default)
    {
        int playerId = PlayerHandler.Instance.GetPlayerCount() - 1;
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { rpcParams.Receive.SenderClientId }
            }
        };
        ReadyClientRpc(playerId, isReadyFromClient, clientRpcParams);
    }

    [ClientRpc]
    private void ReadyClientRpc(int playerId, bool isReadyFromClient, ClientRpcParams clientRpcParams = default)
    {
        OnPlayerReady?.Invoke(playerId, isReadyFromClient);
    }
}
