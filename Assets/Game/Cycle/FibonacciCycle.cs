using UnityEngine;

public class FibonacciCycle  : MonoBehaviour
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
        Quaternion rotation = Quaternion.Euler(yRotation, 90, 0);
        transform.rotation = rotation;
    }
}
