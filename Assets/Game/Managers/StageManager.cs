using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of SceneManager");
            return;
        }
        Instance = this;
    }
    void Start()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadSceneNetwork(string scene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
