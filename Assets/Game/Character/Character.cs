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
    
    [Header("Movement")]
    private float moveSpeed = 80f;

    [Header("Jump")]
    private float jumpForce = 8f;
    private int maxJumps = 2;

    private int jumpCount;
    private bool isGrounded;
    private float horizontalMovement;

    [Header("Ground Check")]
    public Transform groundCheck;
    private float checkerRadius = 0.5f;
    public LayerMask groundLayer;

    [Header("Checkers")]
    private Vector3 checkerTop;
    private Vector3 checkerBot;
    private Vector3 checkerLeft;
    private Vector3 checkerRight;
    
    private Vector3 movement = Vector3.zero;

    private int movingState = 0;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        groundCheck = transform;
        // groundCheck.position = new Vector3(transform.position.x, transform.position.y + collider.bounds.max.y, transform.position.z);
        groundCheck.position = new Vector3(transform.position.x, transform.position.y-1, transform.position.z);
        Debug.Log(transform.position.y);
        Debug.Log(collider.bounds.max.y);
        // groundCheck.parent = transform;
        jumpCount = maxJumps;
    }

    void Update()
    {
        UpdateCheckers();
    }
    
    void FixedUpdate()
    {
        if (movingState == 1)
        {
            rigidbody.AddForce(movement, ForceMode.Force);
        }
        else if (movingState == -1)
        {
            Debug.Log("OMG");
            // rigidbody.AddForce(new Vector3(movement.x*10000, 0, 0), ForceMode.Impulse);
            // rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            // rigidbody.linearVelocity = -rigidbody.linearVelocity;
            // rigidbody.AddForce(new Vector3(movement.x, 0, 0), ForceMode.Impulse);
            rigidbody.AddForce(-movement/20, ForceMode.Impulse);
            movingState = 0;
        }
        // rigidbody.linearVelocity += movement * movingState;
        // rigidbody.linearVelocity = new Vector2(horizontalInput * moveSpeed, rigidbody.linearVelocity.y);
        // rigidbody.AddForce(movement , ForceMode.Force);
        // rigidbody.AddForce( new Vector2(horizontalInput * moveSpeed * 10, 0), ForceMode.Force);
    }


    void UpdateCheckers()
    {
        checkerBot = new Vector3(transform.position.x, collider.bounds.min.y, transform.position.z);
        checkerTop = new Vector3(transform.position.x, collider.bounds.max.y, transform.position.z);
        checkerLeft = new Vector3(collider.bounds.min.x, transform.position.y, transform.position.z);
        checkerRight = new Vector3(collider.bounds.max.x, transform.position.y, transform.position.z);
        
        isGrounded = Physics.CheckSphere(checkerBot, checkerRadius, groundLayer);
        if (isGrounded)
        {
            jumpCount = maxJumps;
        }
    }
    
    public void Move(Vector3 vector)
    {
        horizontalMovement = vector.normalized.x;
        if (horizontalMovement != 0)
        {
            movingState = 1;
            movement = new Vector3(horizontalMovement * moveSpeed, 0, 0);
        }
        else 
        {
            movingState = -1;
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
            Gizmos.DrawWireSphere(checkerBot, checkerRadius);
            Gizmos.DrawWireSphere(checkerTop, checkerRadius);
            Gizmos.DrawWireSphere(checkerLeft, checkerRadius);
            Gizmos.DrawWireSphere(checkerRight, checkerRadius);
        }
    }
}
