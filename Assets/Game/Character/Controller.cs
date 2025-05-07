using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private Character character;
    private void Start()
    {
        character = GetComponent<Character>();
    }
    private void OnMove(InputValue inputValue)
    {
        var value = inputValue.Get<Vector2>();
        character.TryMove(value);
    }
    private void OnJump(InputValue inputValue)
    {
        float value = inputValue.Get<float>();
        character.TryJump(value);
    }
}