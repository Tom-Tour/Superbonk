using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class InputHandler_ : MonoBehaviour
{
    public static InputHandler_ Instance { get; private set; }
    private PlayerInputManager playerInputManager;
    private List<PlayerInput> localPlayerInputs = new List<PlayerInput>();
    private List<PlayerInput> networkPlayerInputs = new List<PlayerInput>();

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;
        playerInputManager.onPlayerLeft += OnPlayerLeft;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }
    private void OnDisable()
    {
        if (NetworkManager.Singleton)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
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
    private void Start()
    {
        StartCoroutine(TryToSpawnPlayers());
    }
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        Debug.Log($"Player {playerIndex} joined using {playerInput.currentControlScheme}");
    }
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        Debug.Log($"Player {playerIndex} leaving using {playerInput.currentControlScheme}");
    }
    public void RegisterLocalPlayerInput(PlayerInput playerInput)
    {
        if (!localPlayerInputs.Contains(playerInput))
        {
            Debug.Log("Registering local player " + playerInput.playerIndex);
            localPlayerInputs.Add(playerInput);
        }
    }
    public void UnregisterLocalPlayerInput(PlayerInput playerInput)
    {
        if (localPlayerInputs.Contains(playerInput))
        {
            Debug.Log("Unregistering local player " + playerInput.playerIndex);
            localPlayerInputs.Remove(playerInput);
        }
    }
    public void RegisterNetworkPlayerInput(PlayerInput playerInput)
    {
        if (!networkPlayerInputs.Contains(playerInput))
        {
            Debug.Log("Registering network player " + playerInput.playerIndex);
            networkPlayerInputs.Add(playerInput);
        }
    }
    public void UnregisterNetworkPlayerInput(PlayerInput playerInput)
    {
        if (networkPlayerInputs.Contains(playerInput))
        {
            Debug.Log("Unregistering network player " + playerInput.playerIndex);
            networkPlayerInputs.Remove(playerInput);
        }
    }
    private void OnClientConnected(ulong clientId)
    {
        //
    }

    
    
    
    
    
    
    
    
    
    
    
    
    

    // TODO
    private IEnumerator TryToSpawnPlayers()
    {
        while (true)
        {
            if (NetworkManager.Singleton.IsListening)
            {
                if (localPlayerInputs != null && localPlayerInputs.Count > 0)
                {
                    Debug.Log("Try to spawn " + localPlayerInputs.Count + " new players in the server..");
                    foreach (PlayerInput playerInput in localPlayerInputs.ToList())
                    { 
                        // playerInput.gameObject.SetActive(false);
                        Destroy(playerInput.gameObject);
                        LobbyManager.Instance.RequestSpawnPlayerServerRpc();
                    }
                }
            }
            else
            {
                if (networkPlayerInputs != null && networkPlayerInputs.Count > 0)
                {
                    Debug.Log("Try to spawn " + networkPlayerInputs.Count + " new players in local..");
                    foreach (PlayerInput playerInput in networkPlayerInputs.ToList())
                    { 
                        // playerInput.gameObject.SetActive(false);
                        Destroy(playerInput.gameObject);
                        GameObject playerCursor = Instantiate(Resources.Load<GameObject>("PlayerCursor"));
                    }
                }
            }
            yield return new WaitForSeconds(4f);
        }
    }
}