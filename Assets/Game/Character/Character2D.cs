using UnityEngine;

public class Character2D : Character
{
    public Rigidbody2D rigidbody2D { get; private set; }
    public Collider2D collider2D { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
    }
}
