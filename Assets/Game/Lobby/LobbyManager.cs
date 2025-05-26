using UnityEngine;
using Unity.Netcode;

public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager Instance { get; private set; }
    
    private void OnEnable()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }
    private void OnDisable()
    {
        if (NetworkManager.Singleton)
        {
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of LobbyManager");
            return;
        }
        Instance = this;
    }

    private void OnServerStarted()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }
    private void OnClientConnected(ulong clientId)
    {
        /*
        Debug.Log("OnClientConnected");
        GameObject playerCursor = Instantiate(Resources.Load<GameObject>("PlayerCursor"));
        NetworkObject networkObject = playerCursor.GetComponent<NetworkObject>();
        networkObject.SpawnAsPlayerObject(clientId, true);
        */
    }

    [ServerRpc(RequireOwnership = false)] public void RequestSpawnPlayerServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        GameObject playerCursor = Instantiate(Resources.Load<GameObject>("PlayerCursor"));
        playerCursor.GetComponent<PlayerCursorController>().isLocal = false;
        NetworkObject networkObject = playerCursor.GetComponent<NetworkObject>();
        networkObject.SpawnAsPlayerObject(clientId, true);
    }
}