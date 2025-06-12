using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    // REFERENCES
    private Cursor cursor;
    private PlayerInput playerInput;
    private Camera camera;
    
    // ROUTINES
    private Coroutine getCameraRoutine;
    
    // INFORMATIVES
    private Vector2 direction = Vector2.zero;
    private bool isUsingMouseAndKeyboard = false;
    private Vector2 lastMousePosition;

    private void Awake()
    {
        cursor = GetComponent<Cursor>();
        playerInput = GetComponent<PlayerInput>();
        camera = Camera.main;
        isUsingMouseAndKeyboard = playerInput.currentControlScheme == "Keyboard&Mouse";
    }

    private IEnumerator GetCameraRoutine()
    {
        while (!camera)
        {
            Camera cam = Camera.main;
            if (cam)
            {
                Debug.Log("Find camera !");
                camera = cam;
                yield break;
            }
            yield return new WaitForSeconds(.1f);
        }
    }
    
    private void Update()
    {
        if (isUsingMouseAndKeyboard)
        {
            if (!camera)
            {
                if (getCameraRoutine == null)
                {
                    getCameraRoutine = StartCoroutine(GetCameraRoutine());
                }
            }
            else
            {
                Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
                if (lastMousePosition != mouseScreenPos)
                {
                    // Vector3 mouseWorldPos = camera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, camera.nearClipPlane));
                    Vector3 mouseWorldPos = camera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, -camera.transform.position.z));
                    mouseWorldPos.z = 0;
                    cursor.Teleport(mouseWorldPos);
                    lastMousePosition = mouseScreenPos;
                } 
            }
        }
    }
    private void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>().normalized;
        cursor.SetDirection(direction);
    }
    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            cursor.Ready();
        }
    }
    private void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            cursor.Ready();
        }
    }

    private void OnCrouch(InputValue value)
    {
        if (value.isPressed)
        {
            cursor.ForceStart();
        }
    }
}
