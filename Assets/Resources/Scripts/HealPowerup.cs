using UnityEngine;

public class HealPowerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private AudioClip _healSFX;

    private void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerAttack>().AddHealth(); //+1 life
            HUDManager.Instance.UpdateLifes();
            SFXManager.Instance.PlaySFX(_healSFX, other.transform.position, 0.6f);
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("Border")) Destroy(gameObject);

    }
}
