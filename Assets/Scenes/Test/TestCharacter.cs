using UnityEngine;

public class TestCharacter : MonoBehaviour
{
    private float speed = 20f;
    private Vector3 direction;
    private void FixedUpdate()
    {
        transform.position += direction * (Time.fixedDeltaTime * speed);
    }
    public void ChangeDirection(Vector2 value)
    {
        direction = new Vector3(value.x, 0f, value.y);
    }
}
