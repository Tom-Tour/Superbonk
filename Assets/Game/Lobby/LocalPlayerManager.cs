using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class LocalPlayerManager : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }
    private void OnDisable()
    {
        PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Test");
        NetworkObject networkObject = playerInput.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            // LobbyManager.Instance.RequestSpawnPlayerServerRpc();
            // networkObject.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
            // networkObject.ChangeOwnership(NetworkManager.Singleton.LocalClientId);
        }
    }
}