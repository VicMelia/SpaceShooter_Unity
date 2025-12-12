using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject _explosionParticle;
    private TextMeshPro _bulletText;
    private Transform _enemyTarget;
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private float _speed = 15f;
    private float _rotationSpeed = 360f;
    private bool _failed = false;
    private float _lifeTime = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _bulletText = GetComponentInChildren<TextMeshPro>();
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyBullet());

    }

    // Update is called once per frame
    void Update()
    {
        if (_failed)
        {
            transform.Rotate(0f, 0f, -_rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (_enemyTarget == null) return;
            Vector2 dir = (_enemyTarget.position - transform.position);
            _moveDirection = dir.normalized;
        }
    }

    private void FixedUpdate()
    {
        if (_failed) return;
        _rb.linearVelocity = _moveDirection * _speed;
    }

    public void SetTarget(Transform target)
    {
        _enemyTarget = target;
    }

    public void SetCharacter(char c)
    {
        _bulletText.text = c.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !_failed)
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            char c = _bulletText.text[0];
            if (CompareCharacter(c, e.currentWord[e.currentIndex]))
            {
                var instance = Instantiate(_explosionParticle, collision.gameObject.transform.position, Quaternion.identity);
                ParticleSystem ps = instance.GetComponent<ParticleSystem>();
                Destroy(ps.gameObject, ps.main.startLifetime.constantMax);
                e.SetNextChar();
                Destroy(gameObject);
            }
            else
            {
                FailProjectile();
            }

        }
        if (collision.gameObject.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }



    private void FailProjectile()
    {
        _failed = true;
        TrailRenderer tr = GetComponentInChildren<TrailRenderer>();
        tr.enabled = false;
        _rb.gravityScale = 1f;

        Vector2 parabolic = new Vector2( //x,y impulse
            Random.Range(-2f, -1f),    
            Random.Range(4f, 5f)      
        );
        _rb.linearVelocity = parabolic;

    }

    private bool CompareCharacter(char c, char ec)
    {
        return c == ec;
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }
}
