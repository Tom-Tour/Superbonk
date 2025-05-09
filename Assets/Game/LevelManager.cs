using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public LevelDB currentLevel;

    void Start()
    {
        LoadLevel(currentLevel);
    }

    public void LoadLevel(LevelDB level)
    {
        SceneManager.LoadScene(level.scene.name);
        
        if (level.music)
        {
            AudioManager.instance.PlayMusic(level.music);
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

