using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class CursorController : NetworkBehaviour
{
    // MODIFIERS
    private float speed = 4;
    
    // INFORMATIVES
    private Vector2 direction = Vector2.zero;
    private bool isReady = false;
    private void Update()
    {
        bool isListening = NetworkManager.Singleton && NetworkManager.Singleton.IsListening;
        if (IsOwner || !isListening)
        {
            transform.Translate(direction * (speed * Time.deltaTime));
        }
    }
    private void OnMove(InputValue value)
    {
        bool isListening = NetworkManager.Singleton && NetworkManager.Singleton.IsListening;
        if (IsOwner || !isListening)
        {
            direction = value.Get<Vector2>().normalized;
            isReady = false;
        }
    }
    private void OnJump(InputValue value)
    {
        bool isListening = NetworkManager.Singleton && NetworkManager.Singleton.IsListening;
        if (IsOwner || !isListening)
        {
            if (value.isPressed)
            {
                isReady = !isReady;
            }
        }
    }
}
