using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }
    [SerializeField] private Transform _lifesTransform;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void UpdateLifes()
    {
        int lifes = FindAnyObjectByType<PlayerAttack>().lifes;

        for (int i = 0; i < _lifesTransform.childCount; i++)
        {
            _lifesTransform.GetChild(i).gameObject.SetActive(i < lifes);
        }
    }
}
