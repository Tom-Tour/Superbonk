using Unity.Netcode;

public class PlayerIdentifier : NetworkBehaviour
{
    private int localPlayerIndex;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Register in PlayerHandler
            // PlayerHandler.Instance?.RegisterPlayer(OwnerClientId, localPlayerIndex);
        }
    }
    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            PlayerHandler.Instance?.UnregisterPlayer(OwnerClientId, localPlayerIndex);
        }
    }

    public void SetPlayerIndex(int playerIndex)
    {
        localPlayerIndex = playerIndex;
    }
}
