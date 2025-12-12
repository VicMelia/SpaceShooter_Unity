using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.5f;
    private AudioSource _musicSource;
    private float _fadeSpeed = 0.05f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _musicSource = GetComponent<AudioSource>();
        _musicSource.clip = backgroundMusic;
        _musicSource.loop = true;
        _musicSource.playOnAwake = false;
        _musicSource.volume = 0f;

        if (backgroundMusic != null) StartCoroutine(IntroSequence());
    }

    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }

    public void PlayGameOverSFX()
    {
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator IntroSequence()
    {
        _musicSource.Play();
        yield return StartCoroutine(FadeVolume(_musicSource, 0f, musicVolume));
        _musicSource.volume = musicVolume;
    }

    private IEnumerator GameOverSequence()
    {
        yield return StartCoroutine(FadeVolume(_musicSource, _musicSource.volume, 0f));

        _musicSource.Stop();
        _musicSource.volume = 0f;
        _musicSource.loop = false;
    }

    private IEnumerator FadeVolume(AudioSource source, float start, float end)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * _fadeSpeed;
            source.volume = Mathf.Lerp(start, end, t);
            yield return null;
        }

        source.volume = end;
    }
}
