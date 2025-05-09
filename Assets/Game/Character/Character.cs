using System.Collections;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Character : MonoBehaviour
{
    private GameManager gameManager;
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
    private int jumpCountMax = 2;

    private int jumpCount;
    private bool isGrounded;
    private float horizontalMovement;

    [Header("Ground Check")]
    public Transform groundCheck;

    private const float CheckerRadius = 0.5f;
    public LayerMask groundLayer;

    [Header("Checkers")]
    private Vector3 checkerTop;
    private Vector3 checkerBot;
    private Vector3 checkerLeft;
    private Vector3 checkerRight;
    
    private Vector3 movement = Vector3.zero;

    private int movingState = 0;
    
    
    // Physic
    private Vector3 velocity = Vector3.zero;
    
    // Gravity
    private float gravityMultiplier = 1;
    private Vector3 gravityDirectionOverride  =  Vector3.zero;
    
    
    private bool isMoving = false;
    
    
    
    
    
    
    private void Awake()
    {
        InitComponents();
        InitVariables();
    }
    
    private void Start()
    {
        InitGameManager();
        gameManager.RegisterCharacter(this);
    }

    void Update()
    {
        UpdateCheckers();
        isGrounded = Physics.CheckSphere(checkerBot, CheckerRadius, groundLayer);
        if (isGrounded)
        {
            jumpCount = jumpCountMax;
        }
    }
    
    void FixedUpdate()
    {
        /*
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
        */

        /*
        velocity.y += gravity * Time.fixedDeltaTime;
        if (velocity.y < maxFallSpeed)
        {
            velocity.y = maxFallSpeed;
        }
        rb.velocity = velocity;
        */
        velocity = Vector3.zero;
        // ApplyGravity();
        ApplyMovement();
        rigidbody.linearVelocity = velocity;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void ApplyGravity()
    {
        if (gravityDirectionOverride != Vector3.zero)
        {
            velocity += gravityDirectionOverride * Mathf.Clamp(LevelData.instance.gravity * gravityMultiplier * Time.fixedDeltaTime, 0, 20 * gravityMultiplier);
        }
        velocity += LevelData.instance.gravityDirection * Mathf.Clamp(LevelData.instance.gravity * gravityMultiplier * Time.fixedDeltaTime, 0, 20 * gravityMultiplier);
    }

    private void ApplyMovement()
    {
        if (!isMoving)
        {
            movement *= .88f;
        }
        velocity += movement;
    }
    
    public void TryMove(Vector2 vector)
    {
        Move(vector);
    }
    private void Move(Vector2 vector)
    {
        horizontalMovement = vector.normalized.x;
        if (horizontalMovement != 0)
        {
            isMoving = true;
            movement = new Vector3(horizontalMovement * moveSpeed * Time.fixedDeltaTime, 0, 0);
        }
        else
        {
            isMoving = false;
        }
    }
    public void TryJump(float jumpValue)
    {
        Debug.Log(jumpValue);
        if (jumpCount > 0)
        {
            Jump();
        }
    }
    private void Jump()
    {
        rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, 0);
        rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        jumpCount--;
    }
    private void InitComponents()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }
    private void InitVariables()
    {
        jumpCount = jumpCountMax;
    }
    private void InitGameManager()
    {
        gameManager = GameManager.instance;
    }
    private void UpdateCheckers()
    {
        checkerBot = new Vector3(transform.position.x, collider.bounds.min.y, transform.position.z);
        checkerTop = new Vector3(transform.position.x, collider.bounds.max.y, transform.position.z);
        checkerLeft = new Vector3(collider.bounds.min.x, transform.position.y, transform.position.z);
        checkerRight = new Vector3(collider.bounds.max.x, transform.position.y, transform.position.z);
    }
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(checkerBot, CheckerRadius);
            Gizmos.DrawWireSphere(checkerTop, CheckerRadius);
            Gizmos.DrawWireSphere(checkerLeft, CheckerRadius);
            Gizmos.DrawWireSphere(checkerRight, CheckerRadius);
        }
    }
}
