using Unity.Netcode;

public class PlayerIdentifier : NetworkBehaviour
{
    public int PlayerIndex { get; private set; }

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
            PlayerHandler.Instance?.UnregisterPlayer(OwnerClientId, PlayerIndex);
        }
    }

    public void SetPlayerIndex(int newPlayerIndex)
    {
        PlayerIndex = newPlayerIndex;
    }
}
