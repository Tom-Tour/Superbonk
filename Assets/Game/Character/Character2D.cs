using UnityEngine;

public class Character2D : Character
{
    public Rigidbody2D rigidbody { get; private set; }
    public Collider2D collider { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }
}
