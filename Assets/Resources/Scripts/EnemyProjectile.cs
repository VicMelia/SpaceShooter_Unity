using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    private float _lifetime = 4f;
    private float _speed = 4.5f;
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Destroy(this, _lifetime);
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.linearVelocity = _moveDirection * _speed;
    }

    public void SetMovementVector(Vector2 movement)
    {
        _moveDirection = movement;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerAttack player = collision.gameObject.GetComponent<PlayerAttack>();
            Instantiate(_particleSystem, player.transform.position, Quaternion.identity);
            player.GetDamage();
            Destroy(gameObject);
            
        }
        else if (collision.gameObject.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }

}
