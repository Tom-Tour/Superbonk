using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Lobby : NetworkBehaviour
{
    private GameObject playerCursorPrefab;
    private List<int> readyCursors = new List<int>();
    
    private void Awake()
    {
        playerCursorPrefab = Resources.Load<GameObject>("NetworkedPlayerCursor");
    }

    public override void OnNetworkSpawn()
    {
        readyCursors.Clear();
    }
    
    public override void OnNetworkDespawn()
    {
        readyCursors.Clear();
    }
    
    private void OnEnable()
    {
        PlayerHandler.OnPlayerRegistered += OnRequestSpawnPlayer;
        Cursor.OnPlayerReady += OnPlayerReady;
    }
    private void OnDisable()
    {
        PlayerHandler.OnPlayerRegistered -= OnRequestSpawnPlayer;
        Cursor.OnPlayerReady -= OnPlayerReady;
    }

    private void OnPlayerReady(int playerId, bool isReady)
    {
        if (NetworkManager.Singleton.IsListening)
        {
            if (isReady)
            {
                RegisterReadyCursorServerRpc(playerId);
            }
            else
            {
                UnregisterReadyCursorServerRpc(playerId);
            }
        }
        else
        {
            if (isReady)
            {
                RegisterReadyCursor(playerId);
            }
            else
            {
                UnregisterReadyCursor(playerId);
            }
        }
    }

    private void OnRequestSpawnPlayer(ulong clientId, int localPlayerIndex)
    {
        Debug.Log($"Player {clientId}.{localPlayerIndex} has been spawned.");
        GameObject cursor = Instantiate(playerCursorPrefab);
        cursor.GetComponent<PlayerIdentifier>().SetPlayerIndex(localPlayerIndex);
        cursor.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RegisterReadyCursorServerRpc(int playerIndex)
    {
        if (!readyCursors.Contains(playerIndex))
        {
            readyCursors.Add(playerIndex);
            if (readyCursors.Count == PlayerHandler.Instance.GetPlayerCount())
            {
                StageManager.Instance.LoadSceneNetwork("Arena");
            }
        }
    }
    private void RegisterReadyCursor(int playerIndex)
    {
        if (!readyCursors.Contains(playerIndex))
        {
            readyCursors.Add(playerIndex);
            if (readyCursors.Count == InputHandler.Instance.PlayerIndexes.Count)
            {
                StageManager.Instance.LoadScene("Arena");
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void UnregisterReadyCursorServerRpc(int playerIndex)
    {
        if (readyCursors.Contains(playerIndex))
        {
            readyCursors.Remove(playerIndex);
        }
    }
    private void UnregisterReadyCursor(int playerIndex)
    {
        if (readyCursors.Contains(playerIndex))
        {
            readyCursors.Remove(playerIndex);
        }
    }
}
