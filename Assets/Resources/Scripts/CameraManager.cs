using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    private bool _hasReachedZero = false;

    private void Update()
    {
        if (_hasReachedZero) return;
        Vector3 pos = transform.position;

        if (pos.y > 0f)
        {
            pos.y -= _speed * Time.deltaTime;
            if (pos.y <= 0f)
            {
                pos.y = 0f;
                OnCameraReachedZero();
            }

            transform.position = pos;
        }
    }

    private void OnCameraReachedZero()
    {
        _hasReachedZero = true;
        GameManager.Instance.FadeIn(GameManager.Instance.introCanvas);
    }
}
