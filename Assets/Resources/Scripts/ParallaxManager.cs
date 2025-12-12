using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    private Material _mat;
    private Vector2 _initialSpeed;
    private float _currentSpeedX;

    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _smoothFactor = 5f;

    private void Start()
    {
        _mat = GetComponent<SpriteRenderer>().material;
        _initialSpeed = _mat.GetVector("_Speed");
        _currentSpeedX = _initialSpeed.x;
    }

    private void Update()
    {
        if (EnemyManager.Instance == null) return;
        float targetSpeedX = CalculateSpeed();
        targetSpeedX = Mathf.Min(targetSpeedX, _maxSpeed);
        _currentSpeedX = Mathf.Lerp(_currentSpeedX, targetSpeedX, Time.deltaTime * _smoothFactor); //lerps to make an smooth transition between speeds
        Vector2 newSpeed = new Vector2(_currentSpeedX, _initialSpeed.y);
        _mat.SetVector("_Speed", newSpeed);
    }

    private float CalculateSpeed()
    {
        return _initialSpeed.x * EnemyManager.Instance.currentSpeedMultiplier;
    }
}
