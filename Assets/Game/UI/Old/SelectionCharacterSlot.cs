using UnityEngine;

public class SelectionCharacterSlot : MonoBehaviour
{
    private int x, y;
    private float gridSize = 4;

    private void Awake()
    {
        x = (int)transform.position.x/4;
        y = (int)transform.position.y/4;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SelectionCharacter.Instance.ClickedOnCharacter(x, y);
    }
}
