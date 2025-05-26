using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using UnityEditor;
using UnityEngine.InputSystem.Users;

public class PlayerCursorLocal : MonoBehaviour
{
    // COMPONENTS
    private SpriteRenderer spriteRenderer;
    private PlayerInput playerInput;
    
    // INFORMATIVES
    public bool isReady { get; private set; } = false;
    private Color color;
    private Vector3 direction;
    
    // MODIFIERS
    private float speed = 12;
    
    void OnEnable()
    {
        if (!NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.OnServerStarted += SwitchToNetwork;
            NetworkManager.Singleton.OnClientConnectedCallback += AskToSwitchToNetwork;
        }
    }
    void OnDisable()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted -= SwitchToNetwork;
            NetworkManager.Singleton.OnClientConnectedCallback -= AskToSwitchToNetwork;
        }
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        if (NetworkManager.Singleton.IsListening)
        {
            AskToSwitchToNetwork();
        }
    }
    private void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
    }

    public void AskToSwitchToNetwork(ulong clientId = 0)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId)
        {
            return;
        }
        SelectionCharacter.Instance.RequestSpawnPlayerServerRpc();
    }

    public void SwitchToNetwork()
    {
        Debug.Log("SwitchToNetwork");
        var oldPlayerInput = playerInput;

        if (!oldPlayerInput.user.valid)
        {
            Debug.LogWarning("InputUser non valide pour ce joueur.");
            return;
        }

        var devices = oldPlayerInput.devices;
        var controlScheme = oldPlayerInput.currentControlScheme;

        // On instancie un nouveau joueur avec les bons devices/controlScheme
        GameObject newPlayer = PlayerInput.Instantiate(
            Resources.Load<GameObject>("PlayerCursor"),
            controlScheme: controlScheme,
            pairWithDevices: devices.ToArray(),
            splitScreenIndex: -1
        ).gameObject;

        var newPlayerInput = newPlayer.GetComponent<PlayerInput>();

        // Vérification
        if (!newPlayerInput.user.valid)
        {
            Debug.LogWarning("Le nouveau PlayerInput a un InputUser invalide.");
            return;
        }

        // Spawner le NetworkObject comme joueur réseau
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        newPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        // Désactiver et supprimer l'ancien
        Destroy(gameObject);
    }


    
    /*
    public void SwitchToNetwork()
    {
        GameObject newPlayerCursor = Instantiate(Resources.Load<GameObject>("PlayerCursor"));
        PlayerInput newPlayerInput = newPlayerCursor.GetComponent<PlayerInput>();
        var newUser = newPlayerInput.user;
        var user = playerInput.user;
        
        user.UnpairDevices();
        newUser.UnpairDevices();
        // user.UnpairDevicesAndRemoveUser();
        // newUser.UnpairDevicesAndRemoveUser();
        
        foreach (var device in user.pairedDevices)
        {
            InputUser.PerformPairingWithDevice(device, newUser);
        }

        ulong clientId = NetworkManager.Singleton.LocalClientId;
        // ulong clientId = 0;
        newPlayerCursor.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        Destroy(gameObject);
        
        newPlayerInput.SwitchCurrentControlScheme(playerInput.currentControlScheme, playerInput.devices[0], playerInput.devices[1]);
        newPlayerInput.ActivateInput();
    }
    */
        
        /*
        user.UnpairDevicesAndRemoveUser();
        
        // Recréer les utilisateurs
        var newUser1 = InputUser.CreateUserWithoutPairedDevices();
        foreach (var device in devices)
            newUser1.PairDevice(device);

        var newUser2 = InputUser.CreateUserWithoutPairedDevices();
        foreach (var device in devices1)
            newUser2.PairDevice(device);

        // Réassigner les users aux PlayerInput
        player1.user = newUser1;
        player2.user = newUser2;
        
        
        
        newPlayerInput.SwitchCurrentControlScheme(playerInput.currentControlScheme, playerInput.devices[0]);
        newPlayerInput.ActivateInput();
        
        ulong clientId = 0;
        newPlayerCursor.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        // playerCursor.GetComponent<NetworkObject>().Spawn();
        Destroy(gameObject);
        */
        
        
        /*
        var user1 = player1.user;
        var user2 = player2.user;

        var devices1 = new List<InputDevice>(user1.pairedDevices);
        var devices2 = new List<InputDevice>(user2.pairedDevices);

        // Détacher les devices
        user1.UnpairDevicesAndRemoveUser();
        user2.UnpairDevicesAndRemoveUser();

        // Recréer les utilisateurs
        var newUser1 = InputUser.CreateUserWithoutPairedDevices();
        foreach (var device in devices2)
            newUser1.PairDevice(device);

        var newUser2 = InputUser.CreateUserWithoutPairedDevices();
        foreach (var device in devices1)
            newUser2.PairDevice(device);

        // Réassigner les users aux PlayerInput
        player1.user = newUser1;
        player2.user = newUser2;
        */
        
        
        
        
        
        
        
        
    
    
    void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>().normalized;
        isReady = false;
    }
    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            isReady = !isReady;
        }
    }
    public void ChangeColor(int colorID)
    {
        spriteRenderer.color = Palette.rainbowColors[colorID];
    }
}
