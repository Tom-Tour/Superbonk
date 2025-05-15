using UnityEngine;

public class LightCycle : MonoBehaviour
{
    private float yRotation;
    private float rotationSpeed = 4f;

    void Awake()
    {
        yRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        yRotation += rotationSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(45f, yRotation, 0f);
        transform.rotation = rotation;
    }
}
