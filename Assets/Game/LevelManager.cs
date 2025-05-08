using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public LevelData currentLevel;

    void Start()
    {
        LoadLevel(currentLevel);
    }

    public void LoadLevel(LevelData level)
    {
        SceneManager.LoadScene(level.scene.name);
        
        if (level.music)
        {
            AudioManager.Instance.PlayMusic(level.music);
        }
        
        for (int i = 0; i < level.spawnPoints.Length; i++)
        {
            Vector2 pos = level.spawnPoints[i];
        }
        
        foreach (var hazard in level.hazards)
        {
            Instantiate(hazard);
        }
    }
}

