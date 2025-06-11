using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class SelectionCharacter : NetworkBehaviour
{
    public static SelectionCharacter Instance { get; private set; }
    private PlayerInputManager playerInputManager;
    private PlayerCursor playerCursorPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of SelectionCharacter");
            return;
        }
        Instance = this;
        playerInputManager = GetComponent<PlayerInputManager>();
        // playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        playerCursorPrefab = playerInputManager.playerPrefab.GetComponent<PlayerCursor>();
    }
    
    private void OnEnable()
    {
        if (IsServer)
        {
            playerInputManager.onPlayerJoined += OnPlayerJoined;
        }
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoined;
    }

    private void OnDisable()
    {
        if (IsServer)
        {
            playerInputManager.onPlayerJoined -= OnPlayerJoined;
        }
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientJoined;
    }

    void OnClientJoined(ulong clientId)
    {
        Debug.Log("Client " + clientId + " joined.");
    }



    [ServerRpc(RequireOwnership = false)] public void RequestSpawnPlayerServerRpc(ServerRpcParams rpcParams = default)
    {
        Debug.Log("AskToSwitchToNetwork");
        /*
        ulong clientId = rpcParams.Receive.SenderClientId;

        Debug.Log($"Spawn du joueur demandé par le client {clientId}");

        GameObject newPlayer = PlayerInput.Instantiate(
            
            Resources.Load<GameObject>("PlayerCursor"),
            controlScheme: "Gamepad", // ou KeyboardAndMouse selon ton usage
            pairWithDevices: null,
            splitScreenIndex: -1
        ).gameObject;

        var netObj = newPlayer.GetComponent<NetworkObject>();
        netObj.SpawnAsPlayerObject(clientId);
        */
    }









    
    void OnPlayerJoined(PlayerInput playerInput)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            return;
        }
        
        /*
        NetworkObject netPlayerCursor = playerInput.GetComponent<NetworkObject>();
        if (!netPlayerCursor.IsSpawned)
        {
            ulong clientId = playerInput.GetComponent<NetworkObject>().OwnerClientId;
            ulong playerId = (ulong)playerInput.playerIndex;
            Debug.Log("playerId " + playerId + " joined.");
            // netPlayerCursor.SpawnAsPlayerObject(playerId);
        }
        */
        
        
        
        
        /*
        ulong clientId = playerInput.GetComponent<NetworkObject>().OwnerClientId;
        NetworkObject netPlayerCursor = Instantiate(playerCursorPrefab).GetComponent<NetworkObject>();
        netPlayerCursor.SpawnAsPlayerObject(clientId);
        Destroy(playerInput.gameObject);
        */
        // netObject.Spawn();
        // netObject.ChangeOwnership(clientId);
        
        
        
        // Instantiate(playerCursorPrefab).GetComponent<NetworkObject>().SpawnWithOwnership((ulong)playerInput.user.id.GetHashCode());
        // Destroy(playerInput.gameObject);
        /*
        Debug.Log("OnPlayerJoined");
        if (!NetworkManager.Singleton.IsConnectedClient && !NetworkManager.Singleton.IsHost)
        {
            return;
        }
        if (NetworkManager.Singleton.IsServer)
        {
            PlayerCursor playerInstance = Instantiate(playerCursorPrefab, Vector3.zero, Quaternion.identity);
            var netObj = playerInstance.GetComponent<NetworkObject>();
            netObj.SpawnWithOwnership((ulong)playerInput.user.id.GetHashCode());
            Destroy(playerInput.gameObject);
        }
        */
    }

    
    
    
    
    
    
    
    
    
    
    
    
    /*
    [ServerRpc] public void RequestSpawnPlayerCursorServerRpc(PlayerCursor playerCursor)
    {
        playerCursor.GetComponent<NetworkObject>().Spawn();
    }
    */
    /*
    [ClientRpc] void ChangePlayerColorClientRpc(PlayerCursor playerCursor,  int newColorID)
    {
        playerCursor.ChangeColor(newColorID);
    }
    */
    public void ClickedOnCharacter(int x, int y)
    {
        Debug.Log("x: " + x + " | y: " + y);
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    /*
    void OnClientJoined(ulong clientId)
    {
        Debug.Log("Client " + clientId + " joined.");
        if (NetworkManager.Singleton.IsServer)
        {
            if (!NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject)
            {
                var playerCursor = Instantiate(playerCursorPrefab);
                playerCursor.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
                int newColorID = Random.Range(0, Palette.rainbowColors.Length);
                ChangePlayerColorRpc(playerCursor, newColorID);
            }
        }
    }

    [ClientRpc]
    void ChangePlayerColorRpc(PlayerCursor playerCursor,  int newColorID)
    {
        playerCursor.ChangeColor(newColorID);
    }
    */
    /*
    void ChangePlayerColorRpc(ulong networkObjectId, int newColorID)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out var netObj))
        {
            var playerCursor = netObj.GetComponent<PlayerCursor>();
            playerCursor.ChangeColor(newColorID);
        }
        else
        {
            Debug.LogWarning($"Objet réseau non trouvé pour ID {networkObjectId}");
        }
    }
    */
    
    /*
    void OnClientJoined(ulong clientId)
    {
        Debug.Log("Client " + clientId + " joined.");

        if (NetworkManager.Singleton.IsServer)
        {
            if (!NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject)
            {
                // TODO
                // PlayerInput playerInput = playerInputManager.JoinPlayer(0);

                PlayerCursor playerCursor = Instantiate(playerCursorPrefab);
                NetworkObject playerCursorNetwork = playerCursor.GetComponent<NetworkObject>();
                playerCursorNetwork.SpawnAsPlayerObject(clientId);
                int newColorID = Random.Range(0, Palette.rainbowColors.Length);
                // ChangePlayerColorRpc(playerCursorNetwork.NetworkObjectId, newColorID);
                ChangePlayerColorRpc(playerCursor, newColorID);
            }
        }
    }
    */
    /*
    void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("OnPlayerJoined");
        if (!NetworkManager.Singleton.IsConnectedClient && !NetworkManager.Singleton.IsHost)
        {
            return;
        }
        if (NetworkManager.Singleton.IsServer)
        {
            PlayerCursor playerInstance = Instantiate(playerCursorPrefab, Vector3.zero, Quaternion.identity);
            var netObj = playerInstance.GetComponent<NetworkObject>();
            netObj.SpawnWithOwnership((ulong)playerInput.user.id.GetHashCode());
            Destroy(playerInput.gameObject);
        }
        
        // test
        if (NetworkManager.Singleton.IsServer)
        {
            PlayerCursor _playerCursor = Instantiate(playerCursor);
            _playerCursor.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerInput.user.id);
            playerInput.gameObject.SetActive(false);
        }
    }
    */
}
