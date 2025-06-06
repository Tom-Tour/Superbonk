using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class UI_NetworkPanel : MonoBehaviour
{
    // UI
    private string buttonStartHostName = "Start Host";
    private string buttonStartServerName = "Start Server";
    private string buttonStartClientName = "Start Client";
    private string buttonStopHostName = "Stop Host";
    private string buttonStopServerName = "Stop Server";
    private string buttonStopClientName = "Stop Client";
    private Button buttonHost, buttonServer, buttonClient;
    private TMP_InputField inputIP, inputPort;
    private bool subscribed  = false;

    // Network
    private UnityTransport transport;
    private ushort port = 7777;
    private string ipAddress = "127.0.0.1";
    
    // Events
    public static event Action OnNetworkStart;
    
    private void OnEnable()
    {
        StartCoroutine(SubscribeToNetwork());
        RegisterButtonListeners();
    }
    private void OnDisable()
    {
        if (NetworkManager.Singleton)
        {
            subscribed = false;
            NetworkManager.Singleton.OnServerStarted -= OnServerStart;
            NetworkManager.Singleton.OnServerStopped -= OnServerStop;
            NetworkManager.Singleton.OnClientStarted -= OnClientStart;
            NetworkManager.Singleton.OnClientStopped -= OnClientStop;
        }
        buttonHost.onClick.RemoveAllListeners();
        buttonServer.onClick.RemoveAllListeners();
        buttonClient.onClick.RemoveAllListeners();
        inputIP.onEndEdit.RemoveAllListeners();
        inputPort.onEndEdit.RemoveAllListeners();
    }
    private void Awake()
    {
        buttonHost = transform.Find("Button_Host")?.GetComponent<Button>();
        buttonServer = transform.Find("Button_Server")?.GetComponent<Button>();
        buttonClient = transform.Find("Button_Client")?.GetComponent<Button>();
        inputIP = transform.Find("Field_IP/InputField_IP")?.GetComponent<TMP_InputField>();
        inputPort = transform.Find("Field_Port/InputField_Port")?.GetComponent<TMP_InputField>();
    }
    private IEnumerator SubscribeToNetwork()
    {
        if (subscribed)
        {
            Debug.LogWarning("Already subscribed.");
            yield break;
        }
        for (int i = 0; i < 10; i++)
        {
            if (NetworkManager.Singleton)
            {
                subscribed = true;
                transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
                NetworkManager.Singleton.OnServerStarted += OnServerStart;
                NetworkManager.Singleton.OnServerStopped += OnServerStop;
                NetworkManager.Singleton.OnClientStarted += OnClientStart;
                NetworkManager.Singleton.OnClientStopped += OnClientStop;
                yield break;
            }
            yield return new WaitForSeconds(.2f);
        }
        Debug.LogError("Can't subscribe to network.");
    }
    private void RegisterButtonListeners()
    {
        if (!buttonHost || !buttonServer || !buttonClient || !inputIP || !inputPort)
        {
            Debug.LogError("Incorrect layout.");
            return;
        }
        buttonHost.onClick.AddListener(() => OnClickNetwork(0));
        buttonServer.onClick.AddListener(() => OnClickNetwork(1));
        buttonClient.onClick.AddListener(() => OnClickNetwork(2));
        inputIP.onValueChanged.AddListener(OnIPChanged);
        inputPort.onValueChanged.AddListener(OnPortChanged);
    }
    private void OnServerStart()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            buttonHost.GetComponentInChildren<Text>().text = buttonStopHostName;
            buttonServer.GetComponentInChildren<Text>().text = buttonStopServerName;
            buttonHost.interactable = true;
            buttonServer.interactable = true;
        }
    }
    private void OnClientStart()
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            buttonClient.GetComponentInChildren<Text>().text = buttonStopClientName;
            buttonClient.interactable = true;
        }
    }
    private void OnServerStop(bool wasHost)
    {
        if (wasHost || NetworkManager.Singleton.IsServer)
        {
            ResetButtons();
        }
    }
    private void OnClientStop(bool wasHost)
    {
        if (!wasHost)
        {
            ResetButtons();
        }
    }
    void ResetButtons()
    {
        buttonHost.GetComponentInChildren<Text>().text = buttonStartHostName;
        buttonServer.GetComponentInChildren<Text>().text = buttonStartServerName;
        buttonClient.GetComponentInChildren<Text>().text = buttonStartClientName;
        buttonHost.interactable = true;
        buttonServer.interactable = true;
        buttonClient.interactable = true;
        inputIP.interactable = true;
        inputPort.interactable = true;
    }
    void OnClickNetwork(int buttonNumber)
    {
        buttonHost.interactable = false;
        buttonServer.interactable = false;
        buttonClient.interactable = false;
        inputIP.interactable = false;
        inputPort.interactable = false;
        if (!NetworkManager.Singleton.IsListening)
        {
            OnNetworkStart?.Invoke();
            StartCoroutine(OnClickNetworkRoutine(buttonNumber));
        }
        else
        {
            NetworkManager.Singleton.Shutdown();
        }
    }
    private IEnumerator OnClickNetworkRoutine(int buttonNumber)
    {
        yield return new WaitForEndOfFrame();
        switch (buttonNumber)
        {
            case 0:
            {
                transport.SetConnectionData("0.0.0.0", port);
                NetworkManager.Singleton.StartHost();
                break;
            }
            case 1:
            {
                transport.SetConnectionData("0.0.0.0", port);
                NetworkManager.Singleton.StartServer();
                break;
            }
            case 2:
            {
                transport.SetConnectionData(ipAddress, port);
                NetworkManager.Singleton.StartClient();
                break;
            }
            default:
            {
                NetworkManager.Singleton.Shutdown();
                break;
            }
        }
        yield return null;
    }
    void OnIPChanged(string newIP)
    {
        ipAddress = newIP;
        Debug.Log("Nouveau IP : " + ipAddress);
    }
    void OnPortChanged(string newPort)
    {
        port = ushort.TryParse(newPort, out var result) ? result : (ushort)7777;
        Debug.Log("Nouveau Port : " + port);
    }
}