using UnityEngine;

public class EnemyDiagonal : Enemy
{
    private Vector2 _direction;
    protected override void Awake()
    {
        base.Awake();
        Transform player = FindAnyObjectByType<PlayerMovement>().transform;
        _direction = (player.position - transform.position).normalized;
        movementVector = _direction;
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg + 180f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

    }
    protected override void FixedUpdate()
    {
        _rb.linearVelocity = movementVector * _speed;
    }
}
