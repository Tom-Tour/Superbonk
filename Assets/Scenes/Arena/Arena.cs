using UnityEngine;
using Unity.Netcode;

public class Arena : NetworkBehaviour
{
    private GameObject CharacterPrefab;
    
    private void Awake()
    {
        CharacterPrefab = Resources.Load<GameObject>("NetworkedKnight");
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
        GameObject character = Instantiate(CharacterPrefab);
        character.GetComponent<PlayerIdentifier>().SetPlayerIndex(localPlayerIndex);
        character.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }
}
