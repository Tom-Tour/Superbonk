using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class SelectionCharacter : MonoBehaviour
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
        playerCursorPrefab = playerInputManager.playerPrefab.GetComponent<PlayerCursor>();
        playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
    }

    
    
    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoined;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientJoined;
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
    
    void OnClientJoined(ulong clientId)
    {
        Debug.Log("Client " + clientId + " joined.");

        if (NetworkManager.Singleton.IsServer)
        {
            if (!NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject)
            { 
                // TODO
                PlayerInput playerInput = playerInputManager.JoinPlayer(0);
                
                PlayerCursor playerCursor = Instantiate(playerCursorPrefab);
                NetworkObject playerCursorNetwork = playerCursor.GetComponent<NetworkObject>();
                playerCursorNetwork.SpawnAsPlayerObject(clientId);
                int newColorID = Random.Range(0, Palette.rainbowColors.Length);
                // ChangePlayerColorRpc(playerCursorNetwork.NetworkObjectId, newColorID);
                ChangePlayerColorRpc(playerCursor, newColorID);
            }
        }
    }

    [ClientRpc]
    void ChangePlayerColorRpc(PlayerCursor playerCursor,  int newColorID)
    {
        playerCursor.ChangeColor(newColorID);
    }
    
    
    
    
    
    
    
    
    
    
    
    void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("OnPlayerJoined");
        /*
        if (NetworkManager.Singleton.IsServer)
        {
            PlayerCursor _playerCursor = Instantiate(playerCursor);
            _playerCursor.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerInput.user.id);
            playerInput.gameObject.SetActive(false);
        }
        */
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
