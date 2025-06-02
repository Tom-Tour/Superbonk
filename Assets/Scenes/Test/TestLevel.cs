using UnityEngine;

public class TestLevel : MonoBehaviour
{
    private Vector3[] ennemisSpawnPoints =
    {
        new Vector3(4, .5f, 0),
        new Vector3(-4, .5f, 0),
        new Vector3(0, .5f, 4),
        new Vector3(0, .5f, -4)
    };
    void Start()
    {
        for (int i = 0; i < ennemisSpawnPoints.Length; i++)
        {
            GameObject ennemis = new GameObject();
            ennemis.name = "Ennemis";
            ennemis.transform.position = ennemisSpawnPoints[i];
            ennemis.AddComponent<TestEnnemi>();
        }
    }
}
