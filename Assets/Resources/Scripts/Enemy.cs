using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private string[] _enemyWords = 
    {
        "apple", "river", "shadow", "planet", "mirror",
        "stone", "breeze", "flower", "light", "forest",
        "mountain", "silver", "cloud", "dream", "energy",
        "motion", "crystal", "flame", "whisper", "golden",
        "night", "storm", "jelly", "ocean", "spirit",
        "bridge", "tiger", "neon", "pulse", "echo",
        "drift", "thunder", "cosmic", "arrow", "frost",
        "comet", "blaze", "marble", "vision", "spark",
        "gravity", "star", "bloom", "phantom", "valley",
        "orbit", "flare", "wave", "ember", "lantern",
        "quake", "solar", "anchor", "mist", "glide",
        "lotus", "prism", "sonic", "nova", "spiral",
        "gleam", "arcane", "dusk", "hollow", "sky",
        "stream", "radiant", "ripple", "tide", "lunar",
        "echoes", "blaze", "motion", "crystal", "ember",
        "spark", "shadow", "frost", "dune", "ripple",
        "phantom", "glow", "crystal", "ember", "pulse",
        "whisper", "blaze", "tide", "motion", "drift",
        "lunar", "sky", "blaze", "crystal", "spark",
        "shadow"
    };
    private TextMeshPro _enemyText;
    public string currentWord;
    public char[] wordChars;
    public int currentIndex = 0;
    [SerializeField] protected float _speed = 5f;
    protected Rigidbody2D _rb;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float minShootTime = 1.5f;
    [SerializeField] private float maxShootTime = 2.25f;

    private Coroutine shootCoroutine;
    protected Vector2 movementVector = Vector2.zero;
    [SerializeField] ParticleSystem _particleSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        SetupWord();
        if(EnemyManager.Instance.Enemies.Count == 0) UpdateWordText(); //initial enemy with yellow text
        EnemyManager.Instance.AddEnemy(this);
        _speed += EnemyManager.Instance.currentSpeedMultiplier; //adds new speed if higher rounds
        _rb = GetComponent<Rigidbody2D>();
        shootCoroutine = StartCoroutine(ShootCoroutine());
        movementVector = Vector2.left;
    }

    protected virtual void Update()
    {
        CheckBounds();
    }

    protected virtual void FixedUpdate()
    {
        if (GameManager.Instance.gameState == GameManager.State.End) _rb.linearVelocity = Vector2.zero;
        else _rb.linearVelocity = movementVector * _speed;
    }

    private void SetupWord()
    {
        _enemyText = GetComponentInChildren<TextMeshPro>();
        int n = Random.Range(0, _enemyWords.Length);
        currentWord = _enemyWords[n];
        _enemyText.text = currentWord;
        wordChars = new char[currentWord.Length];
        for (int i = 0; i < currentWord.Length; i++)
        {
            wordChars[i] = currentWord[i];
        }
    }

    public void SetNextChar()
    {
        currentIndex++;
        if(currentIndex > currentWord.Length-1)
        {
            EnemyManager.Instance.currentKills++;
            EnemyManager.Instance.totalKills++;
            if (EnemyManager.Instance.currentKills % EnemyManager.Instance.killsToRound == 0)
            {
                EnemyManager.Instance.NextRound();
            }
            DestroyEnemy();
        }
        UpdateWordText();
    }
    private void UpdateWordText()
    {
        string remaining = currentWord.Substring(currentIndex);

        if (remaining.Length > 0) //Yellow first letter
        {
            string colored =
                $"<color=yellow>{remaining[0]}</color>" +
                remaining.Substring(1);

            _enemyText.text = colored;
        }
        else
        {
            _enemyText.text = "";
        }
    }

    private void CheckBounds() //Kills enemy if out of bounds
    {
        Vector3 pos = transform.position;

        if (pos.x < -6.22f || pos.x > 6f || pos.y > 4f || pos.y < -4f)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        if (shootCoroutine != null) StopCoroutine(shootCoroutine);
        EnemyManager.Instance.RemoveEnemy(this);

        if (EnemyManager.Instance.Enemies.Count > 0)
        {
            EnemyManager.Instance.Enemies[0].UpdateWordText(); //new enemy as current (yellow text)
        }
        Destroy(gameObject);
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minShootTime, maxShootTime);
            yield return new WaitForSeconds(waitTime);

            Shoot();
        }
    }

    private void Shoot()
    {
        if (GameManager.Instance.gameState == GameManager.State.End) return;
        if (bulletPrefab == null || firePoint == null) return;

        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (bullet.TryGetComponent<EnemyProjectile>(out EnemyProjectile b))
        {
            b.SetMovementVector(movementVector);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerAttack>().GetDamage();
            Instantiate(_particleSystem, transform.position, Quaternion.identity);
            DestroyEnemy();
        }
    }
}
