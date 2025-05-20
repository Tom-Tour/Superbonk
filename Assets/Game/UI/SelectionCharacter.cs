using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class SelectionCharacter : MonoBehaviour
{
    public static SelectionCharacter Instance { get; private set; }
    private PlayerInputManager playerInputManager;
    private PlayerCursor playerCursor;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of SelectionCharacter");
            return;
        }
        Instance = this;
        playerInputManager = GetComponent<PlayerInputManager>();
        playerCursor = playerInputManager.playerPrefab.GetComponent<PlayerCursor>();
        playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
    }

    
    private void OnEnable()
    {
        // playerInputManager.onPlayerJoined += OnPlayerJoined;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoined;
    }

    private void OnDisable()
    {
        // playerInputManager.onPlayerJoined -= OnPlayerJoined;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientJoined;
    }

    void OnClientJoined(ulong clientId)
    {
        Debug.Log("Client " + clientId + " joined.");
    }
    
    
    void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("OnPlayerJoined");
        if (NetworkManager.Singleton.IsServer)
        {
            PlayerCursor _playerCursor = Instantiate(playerCursor);
            _playerCursor.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerInput.user.id);
            playerInput.gameObject.SetActive(false);
        }
    }

    
    
    
    
    
    
    
    
    
    
    [ServerRpc]
    public void RequestSpawnPlayerCursorRpc(PlayerCursor playerCursor)
    {
        playerCursor.GetComponent<NetworkObject>().Spawn();
    }

    
    public void ClickedOnCharacter(int x, int y)
    {
        Debug.Log("x: " + x + " | y: " + y);
    }
}
