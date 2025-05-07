using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    public string name;
    public string description;
    public Sprite thumbnail;
    public Scene scene;
    public AudioClip music;
    public Vector3[] spawnPoints;
    public GameObject[] hazards;
}