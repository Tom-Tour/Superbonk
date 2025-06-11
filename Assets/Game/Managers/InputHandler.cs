using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }
    private PlayerInputManager playerInputManager;
    private List<PlayerInput> playerInputs = new List<PlayerInput>();
    public List<int> PlayerIndexes { get; private set; } = new List<int>();
    public static event Action<int> OnPlayerJoinedNetwork;
    
    
    void OnEnable()
    {
        UI_NetworkPanel.OnNetworkStart += ClearLocalPlayerInput;
        playerInputManager.onPlayerJoined += OnPlayerJoined;
        playerInputManager.onPlayerLeft += OnPlayerLeft;
    }
    void OnDisable()
    {
        UI_NetworkPanel.OnNetworkStart -= ClearLocalPlayerInput;
        if (playerInputManager)
        {
            playerInputManager.onPlayerJoined -= OnPlayerJoined;
            playerInputManager.onPlayerLeft -= OnPlayerLeft;
        }
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of InputHandler in the scene!");
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        playerInputManager = GetComponent<PlayerInputManager>();
    }
    
    
    
    
    
    
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        NetworkObject networkObject = playerInput.gameObject.GetComponent<NetworkObject>();
        if (!networkObject)
        {
            if (!PlayerIndexes.Contains(playerIndex))
            {
                // Debug.Log($"Player {playerIndex} joined using {playerInput.currentControlScheme}");
                PlayerIndexes.Add(playerIndex);
                playerInputs.Add(playerInput);
                if (NetworkManager.Singleton.IsListening)
                {
                    Destroy(playerInput.gameObject);
                    OnPlayerJoinedNetwork?.Invoke(playerIndex);
                }
            }
        }
    }
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        NetworkObject networkObject = playerInput.gameObject.GetComponent<NetworkObject>();
        if (!networkObject)
        {
            if (PlayerIndexes.Contains(playerIndex))
            {
                // Debug.Log($"Player {playerIndex} leaving using {playerInput.currentControlScheme}");
                PlayerIndexes.Remove(playerIndex);
                playerInputs.Remove(playerInput);
            }
        }
    }
    private void ClearLocalPlayerInput()
    {
        if (!NetworkManager.Singleton.IsListening || NetworkManager.Singleton.IsServer)
        {
            Debug.Log($"Clear local player input");
            int playerIndexesCount = PlayerIndexes.Count;
            for (int i = 0; i < playerIndexesCount; i++)
            {
                Destroy(playerInputs[0].gameObject);
            }
        }
        PlayerIndexes.Clear();
    }
}