using NUnit.Framework;
using TMPro;
using UnityEngine;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        EnemyManager.Instance.AddEnemy(this);
        SetupWord();
        UpdateWordText();
        _speed += EnemyManager.Instance.currentSpeedMultiplier; //adds new speed if higher rounds
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        _rb.linearVelocity = Vector2.left * _speed;
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
            //TODO: Word count game manager + 1
            //Comrpobar si se pasa de ronda con %
            EnemyManager.Instance.RemoveEnemy(this);
            Destroy(gameObject);
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
}
