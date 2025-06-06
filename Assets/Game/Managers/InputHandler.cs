using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Networking;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }
    private PlayerInputManager playerInputManager;
    private List<PlayerInput> playerInputs = new List<PlayerInput>();
    private List<int> playerIndexes = new List<int>();
    private List<int> networkedPlayerIndexes = new List<int>();
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
        // NetworkObject networkObject = playerInput.gameObject.GetComponentInParent<NetworkObject>();
        NetworkObject networkObject = playerInput.gameObject.GetComponent<NetworkObject>();
        if (!networkObject)
        {
            if (!playerIndexes.Contains(playerIndex))
            {
                Debug.Log($"Player {playerIndex} joined using {playerInput.currentControlScheme}");
                playerIndexes.Add(playerIndex);
                playerInputs.Add(playerInput);
                if (NetworkManager.Singleton.IsListening)
                {
                    Destroy(playerInput.gameObject);
                    OnPlayerJoinedNetwork?.Invoke(playerIndex);
                }
            }
        }
        else
        {
            if (!networkedPlayerIndexes.Contains(playerIndex))
            {
                Debug.Log($"Player {playerIndex} joined using {playerInput.currentControlScheme} on NETWORK");
                networkedPlayerIndexes.Add(playerIndex);
            }
        }
    }
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        if (!playerInput.gameObject.GetComponent<NetworkObject>())
        {
            if (playerIndexes.Contains(playerIndex))
            {
                Debug.Log($"Player {playerIndex} leaving using {playerInput.currentControlScheme}");
                playerIndexes.Remove(playerIndex);
                playerInputs.Remove(playerInput);
            }
        }
        else
        {
            if (networkedPlayerIndexes.Contains(playerIndex))
            {
                Debug.Log($"Player {playerIndex} leaving using {playerInput.currentControlScheme} on NETWORK");
                networkedPlayerIndexes.Remove(playerIndex);
            }
        }
    }
    private void ClearLocalPlayerInput()
    {
        if (!NetworkManager.Singleton.IsListening || NetworkManager.Singleton.IsServer)
        {
            Debug.Log($"Clear local player input");
            int playerIndexesCount = playerIndexes.Count;
            for (int i = 0; i < playerIndexesCount; i++)
            {
                Destroy(playerInputs[0].gameObject);
            }
        }
        playerIndexes.Clear();
    } 
    
    
    
    
    
    
    
    
    
    
    
    
    /*
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        Debug.Log($"Player {playerIndex} joined using {playerInput.currentControlScheme}");
        if (!playerInputs.Contains(playerInput))
        {
            playerInputs.Add(playerInput);
            if (NetworkManager.Singleton.IsListening)
            {
                if (playerIndexesIgnored.Contains(playerIndex))
                {
                    Debug.Log($"Player {playerIndex} is ignored");
                    return;
                }

                playerInputs.Remove(playerInput);
                AddToIgnoreList(playerIndex);
                OnPlayerJoinedNetwork?.Invoke(playerInput);
            }
        }
    }
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        Debug.Log($"Player {playerIndex} leaving using {playerInput.currentControlScheme}");
        if (playerInputs.Contains(playerInput))
        {
            if (playerIndexesIgnored.Contains(playerIndex))
            {
                Debug.Log($"Player {playerIndex} is ignored");
                return;
            }
            playerInputs.Remove(playerInput);
        }
    }
    private void ClearLocalPlayerInput()
    {
        if (!NetworkManager.Singleton.IsListening || NetworkManager.Singleton.IsServer)
        {
            Debug.Log($"Clear local player input");
            int playerInputsCount = playerInputs.Count;
            for (int i = 0; i < playerInputsCount; i++)
            {
                Destroy(playerInputs[0].gameObject);
                // DestroyImmediate(playerInputs[0].gameObject);
            }
        }
        playerInputs.Clear();
    }

    private void AddToIgnoreList(int playerIndex)
    {
        Debug.Log($"Adding player index {playerIndex} to ignore list");
        playerIndexesIgnored.Add(playerIndex);
    }

    public void RemoveFromIgnoreList(int playerIndex)
    {
        Debug.Log($"Removing player index {playerIndex} to ignore list");
        playerIndexesIgnored.Remove(playerIndex);
    }
    */
}