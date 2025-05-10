using UnityEngine;

public class LevelData : MonoBehaviour
{
    public static LevelData instance;
    public LevelDB data;
    public float gravityForce;
    public Vector3 gravityDirection;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        gravityForce = data.gravityForce;
        gravityDirection = data.gravityDirection;
    }
}
