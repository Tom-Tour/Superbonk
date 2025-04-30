using UnityEngine;

public class Character : MonoBehaviour
{
    // GAME_MANAGER
    private GameManager gameManager;
    
    // REFERENCES
    public Rigidbody rigidbody { get; private set; }
    
    // CHARACTER_MOVEMENT
    private Vector3 moveVelocity, jumpVelocity, pushVelocity;
    private float moveDirection;
    private float moveForceMax = 20f;
    private float jumpForceMax = 20f;
    private float pushForceMax = 40f;
    
    // CHARACTER_JUMP
    private float jumpForce = 20f;
    private float moveForce = 20f;
    
    // MODIFIERS
    public bool canMove { get; set; } = true;
    
    
    
    
    
    
    
    
    
    void Start()
    {
        // REFERENCES
        rigidbody = GetComponent<Rigidbody>();
        
        // GAME_MANAGER
        gameManager = GameManager.instance;
        gameManager.RegisterCharacter(this);
    }
    void FixedUpdate()
    {
        Vector2 totalVelocity = Vector2.zero;
        totalVelocity += Vector2.ClampMagnitude(moveVelocity, moveForceMax);
        totalVelocity += Vector2.ClampMagnitude(jumpVelocity, jumpForceMax);
        totalVelocity += Vector2.ClampMagnitude(pushVelocity, pushForceMax);
        rigidbody.AddForce(totalVelocity, ForceMode.Force);
        if (moveDirection == 0)
        {
            // TODO
            // moveVelocity += new Vector3(-Mathf.Sign(moveVelocity.normalized.x), 0, 0);
            moveVelocity += new Vector3(-Mathf.Sign(moveVelocity.normalized.x), -Mathf.Sign(moveVelocity.normalized.y), -Mathf.Sign(moveVelocity.normalized.z));
        }
        jumpVelocity += new Vector3(-Mathf.Sign(jumpVelocity.normalized.x), -Mathf.Sign(jumpVelocity.normalized.y), -Mathf.Sign(jumpVelocity.normalized.z));
        pushVelocity += new Vector3(-Mathf.Sign(pushVelocity.normalized.x), -Mathf.Sign(pushVelocity.normalized.y), -Mathf.Sign(pushVelocity.normalized.z));
        // jumpVelocity = Vector3.Lerp(jumpVelocity, Vector3.zero, .1f);
        // pushVelocity = Vector3.Lerp(pushVelocity, Vector3.zero, .1f);
    }
    
    public void Move(Vector3 vector)
    {
        moveDirection = vector.normalized.x;
        if (moveDirection != 0)
        {
            moveVelocity = new Vector3(moveDirection * moveForce, 0, 0);
        }
    }

    public void Jump()
    {
        jumpVelocity = new Vector3(0, jumpForce, 0);
    }
}
