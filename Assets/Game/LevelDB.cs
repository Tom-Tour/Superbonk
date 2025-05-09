using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level/Level Data")]
public class LevelDB : ScriptableObject
{
    public string name;
    public string description;
    public float gravityForce;
    public Vector2 gravityDirection;
    public Sprite thumbnail;
    public Scene scene;
    public AudioClip music;
    public Vector3[] spawnPoints;
    public GameObject[] hazards;
}