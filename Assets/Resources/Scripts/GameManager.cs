using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject enemyManagerPrefab;

    [SerializeField] private float _fadeSpeed = 4f;

    public CanvasGroup blackCanvas;
    public CanvasGroup introCanvas;
    public CanvasGroup hudCanvas;
    public CanvasGroup endCanvas;
    public TextMeshProUGUI killText;

    public enum State
    {
        Intro, Menu, Playing, End
    }

    public State gameState = State.Intro;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Cursor.visible = false;
        StartCoroutine(FadeCanvas(blackCanvas, 1f, 0f));
    }

    private void Update()
    {
        if (gameState == State.Menu)
        {
            //Any key
            if (Input.anyKeyDown)
            {
                gameState = State.Playing;
                FadeOut(introCanvas);
                FadeIn(hudCanvas);
                EnemyManager.Instance.ActivateEnemyManager();

            }
        }
        else if (gameState == State.End)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(RestartSequence());
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //Escape: Quit game
        {
            Application.Quit();
        }
    }

    public void FadeIn(CanvasGroup group)
    {
        StartCoroutine(FadeCanvas(group, 0f, 1f));
        if (gameState == State.Intro) gameState = State.Menu;
    }

    public void FadeOut(CanvasGroup group)
    {
        StartCoroutine(FadeCanvas(group, 1f, 0f));
    }
    private IEnumerator FadeCanvas(CanvasGroup group, float start, float end)
    {
        float t = 0f;
        group.alpha = start;

        while (t < 1f)
        {
            t += Time.deltaTime * _fadeSpeed;
            group.alpha = Mathf.Lerp(start, end, t);
            yield return null;
        }

        group.alpha = end;

    }

    public void FinishGame()
    {
        StartCoroutine(FinishGameSequence());
    }

    private IEnumerator FinishGameSequence()
    {
        FindAnyObjectByType<PlayerMovement>().GetComponentInChildren<SpriteRenderer>().enabled = false;
        FindAnyObjectByType<PlayerMovement>().GetComponentInChildren<TrailRenderer>().enabled = false;
        killText.text = "WORDS KILLED: " + EnemyManager.Instance.totalKills.ToString();
        //Time.timeScale = 0f;
        blackCanvas.alpha = 0f;
        endCanvas.alpha = 0f;
        yield return StartCoroutine(FadeCanvas(blackCanvas, 0f, 1f));
        yield return StartCoroutine(FadeCanvas(endCanvas, 0f, 1f));
        gameState = State.End;
    }

    private IEnumerator RestartSequence()
    {
        yield return StartCoroutine(FadeCanvas(endCanvas, 1f, 0f));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
