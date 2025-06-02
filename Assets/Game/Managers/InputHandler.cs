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
    public static event Action OnPlayerJoinedNetwork;
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
        Debug.Log($"Player {playerIndex} joined using {playerInput.currentControlScheme}");
        if (!playerInputs.Contains(playerInput))
        {
            playerInputs.Add(playerInput);
            if (NetworkManager.Singleton.IsListening)
            {
                // TODO Ne pas faire spawn en boucle..
                // PlayerInputManager.instance.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
                OnPlayerJoinedNetwork?.Invoke();
            }
        }
    }
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        Debug.Log($"Player {playerIndex} leaving using {playerInput.currentControlScheme}");
        if (playerInputs.Contains(playerInput))
        {
            playerInputs.Remove(playerInput);
        }
    }
    private void ClearLocalPlayerInput()
    {
        Debug.Log($"Clear local player input");
        int playerInputsCount = playerInputs.Count;
        for (int i = 0; i < playerInputsCount; i++)
        {
            Destroy(playerInputs[0].gameObject);
            // DestroyImmediate(playerInputs[0].gameObject);
        }
        playerInputs.Clear();
    }
}