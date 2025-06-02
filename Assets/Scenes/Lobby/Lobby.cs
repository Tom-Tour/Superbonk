using UnityEngine;
using Unity.Netcode;

public class Lobby : NetworkBehaviour
{
    private GameObject playerCursorPrefab;

    private void Awake()
    {
        playerCursorPrefab = Resources.Load<GameObject>("PlayerCursor");
    }
    private void OnEnable()
    {
        InputHandler.OnPlayerJoinedNetwork += OnPlayerJoinedNetwork;
    }
    private void OnDisable()
    {
        InputHandler.OnPlayerJoinedNetwork -= OnPlayerJoinedNetwork;
    }

    private void OnPlayerJoinedNetwork()
    {
        RequestSpawnPlayerServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)] public void RequestSpawnPlayerServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        GameObject playerCursor = Instantiate(playerCursorPrefab);
        playerCursor.name = playerCursorPrefab.name + "Network";
        NetworkObject networkObject = playerCursor.GetComponent<NetworkObject>();
        networkObject.SpawnAsPlayerObject(clientId, true);
    }
}
