using System;
using Unity.Netcode;
using UnityEngine;
using Game.Networking;
using System.Collections.Generic;

public class PlayerHandler : NetworkBehaviour
{
    public static PlayerHandler Instance { get; private set; }

    private List<PlayerSlot> playerSlots = new List<PlayerSlot>(8);
    private int maxPlayers = 4;
    
    public static event Action<ulong, int> OnPlayerRegistered;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of PlayerHandler in the scene !");
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public override void OnNetworkDespawn()
    {
        playerSlots.Clear();
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
        RequestRegisterPlayerServerRpc(playerIndex);
    }
    [ServerRpc(RequireOwnership = false)]
    private void RequestRegisterPlayerServerRpc(int localPlayerIndex, ServerRpcParams rpcParams = default)
    {
        int playerId = playerSlots.Count;
        if (playerId >= maxPlayers)
        {
            Debug.LogWarning("Max players reached !");
            return;
        }
        ulong clientId = rpcParams.Receive.SenderClientId;
        RegisterPlayer(clientId, localPlayerIndex, playerId, playerId);
    }
    
    private void RegisterPlayer(ulong clientId, int localPlayerIndex, int colorIndex = -1, int teamId = -1)
    {
        PlayerSlot playerSlot = new PlayerSlot(clientId, localPlayerIndex, colorIndex, teamId);
        if (playerSlots.Contains(playerSlot))
        {
            Debug.LogWarning($"Player {clientId}.{localPlayerIndex} already registered.");
            return;
        }
        Debug.Log($"RegisterPlayerServerRpc : {clientId}.{localPlayerIndex}.");
        playerSlots.Add(playerSlot);
        OnPlayerRegistered?.Invoke(clientId, localPlayerIndex);
    }
    
    public void UnregisterPlayer(ulong clientId, int localPlayerIndex, int colorIndex = -1, int teamId = -1)
    {
        PlayerSlot playerSlot = new PlayerSlot(clientId, localPlayerIndex, colorIndex, teamId);
        if (playerSlots.Contains(playerSlot))
        {
            Debug.Log($"UnregisterPlayer : {clientId}.{localPlayerIndex}");
            playerSlots.Remove(playerSlot);
        }
    }
    
    public int GetPlayerValidIndex()
    {
        return playerSlots.Count -1;
    }
}
