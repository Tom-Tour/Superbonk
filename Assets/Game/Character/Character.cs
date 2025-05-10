using System.Collections;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
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
    
    private float moveSpeed = 8f;
    private float inertiaAir = 2f;
    private float inertiaGround = 2f;
    private float jumpForce = 14f;
    private int jumpCountMax = 20;

    private int jumpCount;
    private bool isGrounded;
    private float horizontalMovement;

    private const float CheckerRadius = .2f;
    public LayerMask groundLayer;

    [Header("Checkers")]
    private Vector3 checkerTop;
    private Vector3 checkerBot;
    private Vector3 checkerLeft;
    private Vector3 checkerRight;
    

    private int movingState = 0;
    
    
    // Physic
    private Vector3 velocity = Vector3.zero;
    
    // Gravity
    private float gravityMultiplier = 1f;
    private Vector3 gravityDirectionOverride  =  Vector3.zero;
    
    
    private bool isGravityOverrided = false;
    private bool isMoving = false;
    private bool isJumping = false;
    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;
    private bool isTouchingTopWall = false;
    private Vector3 jumpingForce = Vector3.zero;
    
    
    
    private Vector3 movement = Vector3.zero;
    private Vector3 gravity = Vector3.zero;
    
    
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
        // isGrounded = Physics.CheckSphere(checkerBot, CheckerRadius, groundLayer);
        isGrounded = Physics.CheckBox(checkerBot, new Vector3(0.98f, 0.02f, 0.98f) * .5f, Quaternion.identity,groundLayer);
        if (isGrounded && !isJumping)
        {
            jumpCount = jumpCountMax;
        }
        isTouchingLeftWall = Physics.CheckBox(checkerLeft, new Vector3(0.02f, 0.98f, 0.98f) * .5f, Quaternion.identity,groundLayer);
        isTouchingRightWall = Physics.CheckBox(checkerRight, new Vector3(0.02f, 0.98f, 0.98f) * .5f, Quaternion.identity,groundLayer);
        isTouchingTopWall = Physics.CheckBox(checkerTop, new Vector3(0.98f, 0.02f, 0.98f) * .5f, Quaternion.identity,groundLayer);
    }
    
    void FixedUpdate()
    {
        velocity = Vector3.zero;
        ApplyGravity();
        ApplyMovement();
        ApplyJumpingForce();
        rigidbody.linearVelocity = velocity;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void ApplyGravity()
    {
        float gravityForce = LevelData.instance.gravityForce * gravityMultiplier;
        if (isGrounded)
        {
            gravity = Vector3.zero;
        }
        else
        {
            Vector3 gravityDirectionSource = isGravityOverrided ? gravityDirectionOverride : LevelData.instance.gravityDirection;
            if (gravity == Vector3.zero)
            {
                // gravity = gravityDirectionSource * (LevelData.instance.gravityForce * gravityMultiplier * Time.fixedDeltaTime);
                // gravity = Vector3.down;
                gravity = gravityDirectionSource;
            }
            else
            {
                // gravity *= 1.1f;
                // gravity = Vector3.Lerp(gravity, gravityDirectionSource * (20 * gravityMultiplier), gravityMultiplier * Time.fixedDeltaTime);
                // gravity = Vector3.Lerp(Vector3.zero, gravityDirectionSource * (20 * gravityMultiplier), (Mathf.Abs(gravity.y)+1)/200);
                gravity = Vector3.Lerp(Vector3.zero, gravityDirectionSource * gravityForce, Mathf.Abs(gravity.magnitude)/gravityForce + 1f/(gravityForce*4));
            }
        }

        // gravity = new Vector3(Mathf.Clamp(gravity.x, -20 * gravityMultiplier, 20 * gravityMultiplier), Mathf.Clamp(gravity.y, -20 * gravityMultiplier, 20 * gravityMultiplier), Mathf.Clamp(gravity.z, -20 * gravityMultiplier, 20 * gravityMultiplier));
        gravity = Vector3.ClampMagnitude(gravity, gravityForce);
        velocity += gravity;
    }
    private void ApplyMovement()
    {
        if (isTouchingLeftWall)
        {
            movement = new Vector3(Mathf.Max(movement.x, 0), movement.y, movement.z);
        }
        if (isTouchingRightWall)
        {
            movement = new Vector3(Mathf.Min(movement.x, 0), movement.y, movement.z);
        }
        if (!isTouchingLeftWall && !isTouchingRightWall && isMoving)
        {
            movement = new Vector3(horizontalMovement * moveSpeed, 0, 0);
        }
        if (!isMoving)
        {
            movement.x = Mathf.Lerp(0, Mathf.Sign(movement.x) * moveSpeed, Mathf.Abs(movement.magnitude)/moveSpeed - 1f/(moveSpeed*inertiaGround));
            // movement *= .88f;
            if (movement.magnitude < .2f)
            {
                movement = Vector3.zero;
            }
        }
        velocity += movement;
    }
    private void ApplyJumpingForce()
    {
        if (isTouchingTopWall)
        {
            jumpingForce = Vector3.zero;
        }
        if (isJumping)
        {
            jumpingForce *= .96f;
            if (Mathf.Abs(jumpingForce.magnitude) < Mathf.Abs(gravity.magnitude))
            {
                isJumping = false;
            }
        }
        else
        {
            if (jumpingForce.magnitude < .2f)
            {
                jumpingForce = Vector3.zero;
            }
            else
            {
                jumpingForce *= .8f;
            }
        }
        velocity += jumpingForce;
    }
    
    private void Move(Vector2 vector)
    {
        horizontalMovement = vector.normalized.x;
        if (horizontalMovement != 0)
        {
            isMoving = true;
            movement = new Vector3(horizontalMovement * moveSpeed, 0, 0);
        }
        else
        {
            isMoving = false;
        }
    }
    private void Jump(float value)
    {
        if (value != 0)
        {
            if (jumpCount > 0)
            {
                isJumping = true;
                jumpingForce = -LevelData.instance.gravityDirection * jumpForce;
                jumpCount--;
                gravity = Vector3.zero;
            }
        }
        else
        {
            isJumping = false;
        }
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
        Gizmos.color = Color.red;
        /*
        Gizmos.DrawWireSphere(checkerBot, CheckerRadius);
        Gizmos.DrawWireSphere(checkerTop, CheckerRadius);
        Gizmos.DrawWireSphere(checkerLeft, CheckerRadius);
        Gizmos.DrawWireSphere(checkerRight, CheckerRadius);
        */
        Gizmos.DrawWireCube(checkerBot, new Vector3(0.98f, 0.02f, 0.98f));
        Gizmos.DrawWireCube(checkerTop, new Vector3(0.98f, 0.02f, 0.98f));
        Gizmos.DrawWireCube(checkerLeft, new Vector3(0.02f, 0.98f, 0.98f));
        Gizmos.DrawWireCube(checkerRight, new Vector3(0.02f, 0.98f, 0.98f));
    }
    public void TryJump(float value)
    {
        Jump(value);
    }
    public void TryMove(Vector2 vector)
    {
        Move(vector);
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
}
