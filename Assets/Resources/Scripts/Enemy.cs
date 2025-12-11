using NUnit.Framework;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private string[] _enemyWords = { "hola", "adios", "wtf", "edward" };
    private TextMeshPro _enemyText;
    private string _currentWord;
    public char[] wordChars;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        SetupWord();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void SetupWord()
    {
        _enemyText = GetComponentInChildren<TextMeshPro>();
        int n = Random.Range(0, _enemyWords.Length);
        _currentWord = _enemyWords[n];
        _enemyText.text = _currentWord;
        wordChars = new char[_currentWord.Length];
        for (int i = 0; i < _currentWord.Length; i++)
        {
            wordChars[i] = _currentWord[i];
        }
    }
}
