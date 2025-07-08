using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Character character;
    private void Awake()
    {
        character = GetComponentInParent<Character>();
    }

    private void Start()
    {
        if (character)
        {
            character.OnAttacking += EnableHitbox;
        }
        EnableHitbox(false);
    }

    private void OnDestroy()
    {
        if (character)
        {
            character.OnAttacking -= EnableHitbox;
        }
    }

    private void EnableHitbox(bool enable)
    {
        gameObject.SetActive(enable);
    }
}
