using UnityEngine;
using Unity.Netcode;

public class Lobby : NetworkBehaviour
{
    private GameObject playerCursorPrefab;
    
    private void Awake()
    {
        playerCursorPrefab = Resources.Load<GameObject>("NetworkedPlayerCursor");
    }
    private void OnEnable()
    {
        PlayerHandler.OnPlayerRegistered += OnRequestSpawnPlayer;
    }
    private void OnDisable()
    {
        PlayerHandler.OnPlayerRegistered -= OnRequestSpawnPlayer;
    }

    private void OnRequestSpawnPlayer(ulong clientId, int localPlayerIndex)
    {
        Debug.Log($"Player {clientId}.{localPlayerIndex} has been spawned.");
        GameObject cursor = Instantiate(playerCursorPrefab);
        cursor.GetComponent<PlayerIdentifier>().SetPlayerIndex(localPlayerIndex);
        cursor.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }
}
