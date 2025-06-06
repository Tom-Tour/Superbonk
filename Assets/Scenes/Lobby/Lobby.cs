using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Game.Networking;

public class Lobby : NetworkBehaviour
{
    private GameObject playerCursorPrefab;
    private List<PlayerSlot> players = new List<PlayerSlot>(8);
    private List<int> playerIndexes = new List<int>(4);

    
    private void Awake()
    {
        playerCursorPrefab = Resources.Load<GameObject>("NetworkedPlayerCursor");
    }
    private void OnEnable()
    {
        InputHandler.OnPlayerJoinedNetwork += OnPlayerJoinedNetwork;
    }
    private void OnDisable()
    {
        InputHandler.OnPlayerJoinedNetwork -= OnPlayerJoinedNetwork;
    }

    private void OnPlayerJoinedNetwork(int playerIndex)
    {
        RequestSpawnPlayerServerRpc(playerIndex);
    }
    [ServerRpc(RequireOwnership = false)]
    private void RequestSpawnPlayerServerRpc(int localPlayerIndex, ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        GameObject cursor = Instantiate(playerCursorPrefab);
        // GameObject cursor = PlayerInput.Instantiate(playerCursorPrefab).gameObject;
        NetworkObject networkObject = cursor.GetComponent<NetworkObject>();
        // networkObject.SpawnWithOwnership(clientId);
        networkObject.SpawnAsPlayerObject(clientId);
    }
    /*
    private void OnPlayerJoinedNetwork(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        if (!NetworkManager.Singleton.IsListening || NetworkManager.Singleton.IsServer)
        {
            Destroy(playerInput.gameObject);
        }
        RequestSpawnPlayerServerRpc(playerIndex);
        // InputHandler.Instance.RemoveFromIgnoreList(playerIndex);
    }
    */
    /*
    [ServerRpc(RequireOwnership = false)]
    private void RequestSpawnPlayerServerRpc(int localPlayerIndex, ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        if (!NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId))
        {
            Debug.LogWarning($"[SPAWN FAILED] Client {clientId} not connected.");
            return;
        }

        // Total players already at max
        if (players.Count >= 8)
        {
            Debug.LogWarning($"[SPAWN FAILED] Maximum player count (8) reached.");
            return;
        }

        // Client already has 4 players?
        int clientCount = players.Count(p => p.ClientId == clientId);
        if (clientCount >= 4)
        {
            Debug.LogWarning($"[SPAWN FAILED] Client {clientId} already has 4 players.");
            return;
        }

        // Already registered same player?
        if (players.Any(p => p.ClientId == clientId && p.LocalIndex == localPlayerIndex))
        {
            Debug.Log($"[SPAWN IGNORED] Player {clientId}:{localPlayerIndex} already spawned.");
            return;
        }

        // Add player slot
        players.Add(new PlayerSlot(clientId, localPlayerIndex));

        // Instantiate and spawn
        GameObject cursor = Instantiate(playerCursorPrefab);
        var netObj = cursor.GetComponent<NetworkObject>();
        netObj.SpawnWithOwnership(clientId);

        Debug.Log($"[SPAWN OK] Player {clientId}:{localPlayerIndex} spawned. Total: {players.Count}");
    }
    */
    
    
    
    /*
    private void OnPlayerJoinedNetwork(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        if (NetworkManager.Singleton.IsListening && !NetworkManager.Singleton.IsServer)
        {
            return;
        }
        if (playerIndexes.Count >= 4)
        {
            Debug.LogWarning("Max local players reached. Ignoring new player input.");
            Destroy(playerInput.gameObject);
            return;
        }
        if (playerIndexes.Contains(playerIndex))
        {
            Debug.LogWarning($"Player index {playerIndex} already registered locally.");
            Destroy(playerInput.gameObject);
            return;
        }

        // Sinon, on ajoute l'index, on demande au serveur d'instancier, et on nettoie l'input
        playerIndexes.Add(playerIndex);
        RequestSpawnPlayerServerRpc(playerIndex);
        InputHandler.Instance.RemoveFromIgnoreList(playerIndex);
    }
    */
    /*
    [ServerRpc(RequireOwnership = false)]
    private void RequestSpawnPlayerServerRpc(int localPlayerIndex, ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        if (!NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId))
        {
            Debug.LogWarning($"[SPAWN FAILED] Client {clientId} not connected.");
            return;
        }

        // Total players already at max
        if (players.Count >= 8)
        {
            Debug.LogWarning($"[SPAWN FAILED] Maximum player count (8) reached.");
            return;
        }

        // Client already has 4 players?
        int clientCount = players.Count(p => p.ClientId == clientId);
        if (clientCount >= 4)
        {
            Debug.LogWarning($"[SPAWN FAILED] Client {clientId} already has 4 players.");
            return;
        }

        // Already registered same player?
        if (players.Any(p => p.ClientId == clientId && p.LocalIndex == localPlayerIndex))
        {
            Debug.Log($"[SPAWN IGNORED] Player {clientId}:{localPlayerIndex} already spawned.");
            return;
        }

        // Add player slot
        players.Add(new PlayerSlot(clientId, localPlayerIndex));

        // Instantiate and spawn
        GameObject cursor = Instantiate(playerCursorPrefab);
        var netObj = cursor.GetComponent<NetworkObject>();
        netObj.SpawnWithOwnership(clientId);

        Debug.Log($"[SPAWN OK] Player {clientId}:{localPlayerIndex} spawned. Total: {players.Count}");
    }
    */
}
