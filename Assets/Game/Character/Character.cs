using UnityEngine;
using Unity.Netcode;

// Bug à corriger :
// Le personnage s'enfonce dans le sol à l'atterrissage avec une vélocité un peu trop forte, arrêtant complètement l'inertie.
// Le personnage peut se retrouver coincé dans certains angles.
// Le personnage clip dans le sol quand il rentre dans le sol avec encore un peu de jumpingForce.
// Utiliser deltaTime sur toutes les forces ?

public class Character : NetworkBehaviour
{
    private GameManager gameManager;
    public Rigidbody rigidbody { get; private set; }
    public Collider collider { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    
    
    // MODIFIERS
    private int jumpCountMax = 20;
    private float jumpForce = 14f;
    private float moveSpeed = 480f;
    private float inertiaAir = .2f;
    private float inertiaGround = .2f;
    private float gravityMultiplier = 1f;
    private float rangeRadius = 1f;
    private Vector3 gravityDirectionOverride = Vector3.zero;

    // INFORMATIVES
    private float horizontalMovement;
    private int jumpCount;
    public bool canMove { get; private set; } = true;
    public bool isGrounded { get; private set; } = false;
    public bool isMoving { get; private set; } = false;
    public bool isJumping { get; private set; } = false;
    public bool isTouchingLeftWall { get; private set; } = false;
    public bool isTouchingRightWall { get; private set; } = false;
    public bool isTouchingTopWall { get; private set; } = false;
    public bool isGravityOverrided { get; private set; } = false;
    private Vector3 velocity = Vector3.zero;
    private Vector3 jumpingForce = Vector3.zero;
    private Vector3 movement = Vector3.zero;
    private Vector3 gravity = Vector3.zero;
    private Vector3 gravityDirectionSource = Vector3.zero;

    // OTHERS
    public LayerMask groundLayer;
    
    // CHECKERS
    private const float CheckerRadius = .2f;
    private Vector3 checkerTop;
    private Vector3 checkerBot;
    private Vector3 checkerLeft;
    private Vector3 checkerRight;
    
    protected virtual void Awake()
    {
        InitComponents();
        InitVariables();
    }
    
    private void Start()
    {
        InitGameManager();
        gameManager.RegisterCharacter(this);
        UpdateGravity();
    }

    void Update()
    {
        UpdateCheckers();
        // DrawRange();
    }
    
    void FixedUpdate()
    {
        velocity = Vector3.zero;
        ApplyMovement();
        ApplyJumpingForce();
        ApplyGravity();
        rigidbody.linearVelocity = velocity;
        if (transform.position.z != 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    private void ApplyMovement()
    {
        if ((isTouchingLeftWall && movement.x < 0) || (isTouchingRightWall && movement.x > 0))
        {
            movement.x = 0f;
        }
        if (isMoving && !isTouchingLeftWall && !isTouchingRightWall)
        {
            movement.x = horizontalMovement * moveSpeed;
        }
        else if (!isMoving)
        {
            float inertia = isGrounded ? inertiaGround : inertiaAir;
            if (inertia <= 0f)
            {
                movement.x = 0f;
            }
            else
            {
                float decelerationRate = moveSpeed / inertia;
                movement.x = Mathf.MoveTowards(movement.x, 0f, decelerationRate * Time.deltaTime);
            }
        }
        Vector3 deltaMovement = movement * Time.deltaTime;
        if (deltaMovement.magnitude < 0.01f)
        {
            deltaMovement = Vector3.zero;
        }
        velocity += deltaMovement;
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
    private void ApplyGravity()
    {
        // TODO
        float gravityForce = LevelData.instance.gravityForce * gravityMultiplier;
        if (isGrounded)
        {
            gravity = Vector3.zero;
        }
        else
        {
            if (gravity == Vector3.zero)
            {
                gravity = gravityDirectionSource;
            }
            else
            {
                gravity = Vector3.Lerp(Vector3.zero, gravityDirectionSource * gravityForce, Mathf.Abs(gravity.magnitude)/gravityForce + 1f/(gravityForce*4));
            }
        }
        gravity = Vector3.ClampMagnitude(gravity, gravityForce);
        velocity += gravity;
    }
    
    private void Move(Vector2 vector)
    {
        horizontalMovement = vector.normalized.x;
        if (horizontalMovement != 0)
        {
            isMoving = true;
            movement = new Vector3(horizontalMovement * moveSpeed, 0, 0);
            
            // spriteRenderer.flipX = horizontalMovement < 0;
            float newRotation = horizontalMovement > 0 ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0, newRotation, 0);

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

    private void DrawRange()
    {
        Vector3 center = transform.position;
        Utils.DrawCircle(center, rangeRadius, Color.darkGreen);
        Vector3 point = new Vector3(6f, 6f, 0);
        Vector3 clampedPoint = Utils.ClampPoint(point, center, rangeRadius);
        Debug.Log("Clamped Point: " + clampedPoint);
    }
    private void UpdateCheckers()
    {

        checkerBot = new Vector3(transform.position.x, collider.bounds.min.y, transform.position.z);
        checkerTop = new Vector3(transform.position.x, collider.bounds.max.y, transform.position.z);
        checkerLeft = new Vector3(collider.bounds.min.x, transform.position.y, transform.position.z);
        checkerRight = new Vector3(collider.bounds.max.x, transform.position.y, transform.position.z);

        if (!isJumping)
        {
            isGrounded = Physics.CheckBox(checkerBot, new Vector3(0.98f, 0.02f, 0.98f) * .5f, Quaternion.identity,groundLayer);
            if (isGrounded)
            {
                jumpCount = jumpCountMax;
            }
        }
        else
        {
            isGrounded = false;
        }

        isTouchingLeftWall = Physics.CheckBox(checkerLeft, new Vector3(0.02f, 0.98f, 0.98f) * .5f, Quaternion.identity,groundLayer);
        isTouchingRightWall = Physics.CheckBox(checkerRight, new Vector3(0.02f, 0.98f, 0.98f) * .5f, Quaternion.identity,groundLayer);
        isTouchingTopWall = Physics.CheckBox(checkerTop, new Vector3(0.98f, 0.02f, 0.98f) * .5f, Quaternion.identity,groundLayer);
    }
    /*
    private void UpdateCheckers()
    {
        float angle = Vector2.SignedAngle(Vector2.down, gravityDirectionSource);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);

        Bounds bounds = collider.bounds;
        Vector3 center = bounds.center;
        Vector3 size = bounds.size;

        // Directions locales du personnage selon sa rotation
        Vector3 down = -transform.up;
        Vector3 up = transform.up;
        Vector3 right = transform.right;
        Vector3 left = -transform.right;

        checkerBot = center + down * (size.y / 2f);
        checkerTop = center + up * (size.y / 2f);
        checkerLeft = center + left * (size.x / 2f);
        checkerRight = center + right * (size.x / 2f);

        Vector3 checkerSizeBotTop = (right * (size.x * 0.98f) + transform.forward * (size.z * 0.98f)) * 0.5f;
        Vector3 checkerSizeLeftRight = (up * (size.y * 0.98f) + transform.forward * (size.z * 0.98f)) * 0.5f;

        if (!isJumping)
        {
            isGrounded = Physics.CheckBox(checkerBot, checkerSizeBotTop, transform.rotation, groundLayer);
            if (isGrounded)
            {
                jumpCount = jumpCountMax;
            }
        }
        else
        {
            isGrounded = false;
        }

        isTouchingTopWall = Physics.CheckBox(checkerTop, checkerSizeBotTop, transform.rotation, groundLayer);
        isTouchingLeftWall = Physics.CheckBox(checkerLeft, checkerSizeLeftRight, transform.rotation, groundLayer);
        isTouchingRightWall = Physics.CheckBox(checkerRight, checkerSizeLeftRight, transform.rotation, groundLayer);
    }
    */
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(checkerBot, new Vector3(0.98f, 0.02f, 0.98f));
        Gizmos.DrawWireCube(checkerTop, new Vector3(0.98f, 0.02f, 0.98f));
        Gizmos.DrawWireCube(checkerLeft, new Vector3(0.02f, 0.98f, 0.98f));
        Gizmos.DrawWireCube(checkerRight, new Vector3(0.02f, 0.98f, 0.98f));
        Gizmos.DrawWireSphere(transform.position, 1f);
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
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void InitVariables()
    {
        jumpCount = jumpCountMax;
    }
    private void InitGameManager()
    {
        gameManager = GameManager.instance;
    }

    private void UpdateGravity()
    {
        gravityDirectionSource = isGravityOverrided ? gravityDirectionOverride.normalized : LevelData.instance.gravityDirection.normalized;
    }
}
