using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private AudioClip _getDamageClip;
    [SerializeField] private AudioClip _shotClip;

    private SpriteRenderer _spriteRenderer;

    public int lifes = 3;
    private float _blinkInterval = 0.1f;
    private float _blinkDuration = 1.5f;
    private bool isInvencible = false;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.State.End) return;
        
        //Get any key
        for (KeyCode key = KeyCode.A; key <= KeyCode.Z; key++)
        {
            if (Input.GetKeyDown(key))
            {
                Debug.Log("Letra pulsada: " + key);
                if (EnemyManager.Instance.CurrentEnemy != null) SetProjectile(key);

            }
        }

    }

    private void SetProjectile(KeyCode key)
    {
        char letter = key.ToString().ToLower()[0];
        var p = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        if (p.TryGetComponent<Projectile>(out Projectile projectile))
        {
            SFXManager.Instance.PlaySFX(_shotClip, transform.position, 0.6f);
            projectile.SetTarget(EnemyManager.Instance.CurrentEnemy.transform);
            projectile.SetCharacter(letter);
        }
    }

    public void GetDamage()
    {
        if (isInvencible) return;
        isInvencible = true;
        SFXManager.Instance.PlaySFX(_getDamageClip, transform.position, 1f);
        lifes--;
        HUDManager.Instance.UpdateLifes();
        if(lifes == 0)
        {
            GameManager.Instance.FinishGame();
        }
        else
        {
            StartCoroutine(DamageBlink());
        }
    }

    public void AddHealth() //Power-up heal
    {
        lifes++;
        if (lifes > 3) lifes = 3;
        HUDManager.Instance.UpdateLifes();
    }
    private IEnumerator DamageBlink()
    {
        float elapsed = 0f;
        while (elapsed < _blinkDuration)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(_blinkInterval);
            elapsed += _blinkInterval;
        }
        _spriteRenderer.enabled = true;
        isInvencible = false;
    }
}
