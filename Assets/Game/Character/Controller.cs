using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void OnMove(InputValue value)
    {
        Vector2 vector = value.Get<Vector2>();
        character.Move(vector);
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            character.Jump();
        }
    }
}
