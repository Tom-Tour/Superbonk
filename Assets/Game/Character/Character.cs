using UnityEngine;

public class Character : MonoBehaviour
{
    // GAME_MANAGER
    private GameManager gameManager;
    
    // REFERENCES
    public Rigidbody rigidbody { get; private set; }
    public Collider collider { get; private set; }
    
    // CHARACTER_MOVEMENT
    private Vector3 moveVelocity, jumpVelocity, pushVelocity;
    private float moveDirection;
    private float moveForceMax = 1f;
    private float jumpForceMax = 1f;
    private float pushForceMax = 1f;
    
    // CHARACTER_JUMP
    // private float jumpForce = 5f;
    private float moveForce = 1f;

    private float dampingForce = 40;
    
    // MODIFIERS
    public bool canMove { get; set; } = true;
    
    
    
    
    
    
    
    
    /*
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
        rigidbody.AddForce(totalVelocity, ForceMode.Impulse);
        // Freinage progressif vers zéro
        if (moveDirection == 0)
        {
            moveVelocity = Vector3.MoveTowards(moveVelocity, Vector3.zero, dampingForce * Time.deltaTime);
        }

        jumpVelocity = Vector3.MoveTowards(jumpVelocity, Vector3.zero, dampingForce * Time.deltaTime);
        pushVelocity = Vector3.MoveTowards(pushVelocity, Vector3.zero, dampingForce * Time.deltaTime);

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
    */
    
    [Header("Movement")]
    public float moveSpeed = 8f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public int maxJumps = 2;

    private int jumpCount;
    private bool isGrounded;
    private float horizontalInput;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        groundCheck = transform;
        groundCheck.position = new Vector3(transform.position.x, transform.position.y + collider.bounds.max.y, transform.position.z);
        // groundCheck.parent = transform;
        jumpCount = maxJumps;
    }

    void Update()
    {

        // Ground check
        CheckGround();
    }

    void FixedUpdate()
    {
        rigidbody.linearVelocity = new Vector2(horizontalInput * moveSpeed, rigidbody.linearVelocity.y);
    }

    public void Move(Vector3 vector)
    {
        moveDirection = vector.normalized.x;
        horizontalInput = moveDirection;
    }
    
    
    void CheckGround()
    {
        // isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            jumpCount = maxJumps;
        }
    }

    public void Jump()
    {
        if (jumpCount > 0)
        {
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, 0); // Cancel vertical velocity
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            jumpCount--;
        }
    }
    
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
