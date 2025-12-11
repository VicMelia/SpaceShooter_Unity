using NUnit.Framework;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private string[] _enemyWords = { "hola", "adios", "wtf", "edward" };
    private TextMeshPro _enemyText;
    public string currentWord;
    public char[] wordChars;
    public int currentIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        EnemyManager.Instance.AddEnemy(this);
        SetupWord();
        UpdateWordText();
    }

    // Update is called once per frame
    private void Update()
    {
        
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
