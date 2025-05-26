using UnityEngine;
using UnityEngine.InputSystem;

public class TestController : MonoBehaviour
{
    private TestCharacter testCharacter;

    private void Awake()
    {
        testCharacter = GetComponent<TestCharacter>();
    }
    private void OnMove(InputValue value)
    {
        testCharacter.ChangeDirection(value.Get<Vector2>().normalized);
    }
}
