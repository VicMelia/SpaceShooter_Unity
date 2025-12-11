using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private TrailRenderer _trailRenderer;
    private float _speed = 3f;
    private float _verticalInput = 0f;
    private float _horizontalInput = 0f;
    private float _rotationSpeed = 8f;
    private const float ROT_DEGREES = 40f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _verticalInput = 0f;
        _horizontalInput = 0f;
        float rotDegrees = 0f;

        //Vertical movement
        if (Input.GetKey(KeyCode.UpArrow)) {
            _verticalInput = 1f;
            rotDegrees = ROT_DEGREES;
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            _verticalInput = -1f;
            rotDegrees = -ROT_DEGREES;
        }

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, rotDegrees);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);

        // Horizontal movement
        if (Input.GetKey(KeyCode.RightArrow)) _horizontalInput = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow)) _horizontalInput = -1f;

    }

    private void FixedUpdate()
    {
        Vector2 finalVelocity = new Vector2(_horizontalInput, _verticalInput) * _speed;
        _rb.linearVelocity = finalVelocity;
    }
}
