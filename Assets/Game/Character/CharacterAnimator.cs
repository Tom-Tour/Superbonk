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
        /*
        AnimatorClipInfo[] animatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (animatorClipInfo.Length > 0)
        {
            string clipName = animatorClipInfo[0].clip.name;
            bool attacking = clipName == "Attack1" || clipName == "Attack2" || clipName == "Attack3";
            if (attacking)
            {
                return;
            }
        }
        */
        if (character.IsAttacking)
        {
            string attackName = "Attack" + character.AttackingState;
            animator.Play(attackName);
        }
        else
        {
            if (character.IsGrounded)
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
}
