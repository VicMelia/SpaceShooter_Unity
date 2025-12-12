using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;

    private int _lifes = 3;

    // Update is called once per frame
    private void Update()
    {
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
            projectile.SetTarget(EnemyManager.Instance.CurrentEnemy.transform);
            projectile.SetCharacter(letter);
        }
    }

    public void GetDamage()
    {
        _lifes--;
        if(_lifes == 0)
        {
            GameManager.Instance.FinishGame();
        }
    }
}
