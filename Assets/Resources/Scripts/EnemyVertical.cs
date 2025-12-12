using UnityEngine;

public class EnemyVertical : Enemy
{

    protected override void Awake()
    {
        base.Awake();
        movementVector = Vector2.down;
    }
    protected override void FixedUpdate()
    {
        _rb.linearVelocity = movementVector * _speed;
    }
}
