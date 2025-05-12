using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.Play($"Idle");
    }
}
