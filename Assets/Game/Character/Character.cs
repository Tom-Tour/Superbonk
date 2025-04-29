using UnityEngine;

public class Character : MonoBehaviour
{
    // GAME_MANAGER
    private GameManager gameManager;
    
    // REFERENCES
    public Rigidbody rigidbody { get; private set; }
    
    // CHARACTER_MOVEMENT
    private float speed = 40f;
    private float acceleration = 1f;
    private float deceleration = 1f;
    
    // VELOCITY
    private float velocity_maximum = 10f;
    
    // MODIFIERS
    public bool canMove { get; set; } = true;
    
    // INFORMATIVES
    private float horizontal;
    
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
        rigidbody.linearVelocity += new Vector3(horizontal * speed, 0f, 0f);
        rigidbody.linearVelocity = Vector3.ClampMagnitude(rigidbody.linearVelocity, velocity_maximum);
    }
    public void Move(Vector3 vector)
    {
        horizontal = vector.normalized.x;
    }
}
