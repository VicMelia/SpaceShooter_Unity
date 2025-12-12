using UnityEngine;

public class EnemyVertical : Enemy
{
    protected override void FixedUpdate()
    {
        _rb.linearVelocity = Vector2.down * _speed;
    }
}
