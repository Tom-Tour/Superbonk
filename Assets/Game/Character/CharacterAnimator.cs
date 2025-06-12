using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Character character;
    private Animator animator;
    void Awake()
    {
        character = GetComponentInParent<Character>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (character.isGrounded)
        {
            if (character.IsMoving)
            {
                animator.Play($"Run");
            }
            else
            {
                animator.Play($"Idle");
            }
        }
        else
        {
            if (character.IsJumping)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                {
                    animator.Play($"Jump");
                }
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
                {
                    animator.Play($"Fall");
                }
            }
        }
    }
}
