using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
    public Enemy CurrentEnemy { get; private set; }

    public TextMeshProUGUI roundText;

    //Stats
    public int currentRound = 1;
    public int currentKills = 0;
    public int totalKills = 0;
    public int killsToRound = 5;
    public float currentSpeedMultiplier = 1f;

    //Spawns
    private float _cooldownSpawn = 5f;
    [SerializeField] private GameObject _enemy1;
    [SerializeField] private GameObject _enemy2;
    [SerializeField] private GameObject _enemy3;
    [SerializeField] private Transform _horizontalSpawns;
    [SerializeField] private Transform _verticalSpawns;
    [SerializeField] private Transform _diagonalSpawns;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        CurrentEnemy = null;

    }
    public void AddEnemy(Enemy enemy)
    {
        if (!Enemies.Contains(enemy))
        {
            Enemies.Add(enemy);
        }
    }
    public void RemoveEnemy(Enemy enemy)
    {
        if (Enemies.Contains(enemy))
        {
            Enemies.Remove(enemy);
        }
    }

    void Update()
    {
        if(CurrentEnemy == null && Enemies.Count != 0)
        {
            CurrentEnemy = Enemies[0];
        }
    }

    public void ActivateEnemyManager() //from game manager
    {
        StartCoroutine(SpawnEnemy());
        roundText.text = "Round: " + currentRound.ToString();
    }

    public void NextRound()
    {
        Debug.Log("Se pasa de ronda");
        _cooldownSpawn -= 0.5f;
        if (_cooldownSpawn < 0.8f) _cooldownSpawn = 0.8f;
        currentSpeedMultiplier += 0.2f;

        currentKills = 0;
        killsToRound = Random.Range(4, 7); //next round target kills become random
        currentRound++;
        roundText.text = "Round: " + currentRound.ToString();
    }

    private IEnumerator SpawnEnemy()
    {
        float cd = Random.Range(_cooldownSpawn - 0.2f, _cooldownSpawn);
        yield return new WaitForSeconds(cd);
        Spawn();
        StartCoroutine(SpawnEnemy());
    }

    private void Spawn()
    {
        int r = Random.Range(0, 3);
        GameObject prefabToSpawn = null;
        Transform spawnTransform = null;
        switch (r)
        {
            case 0:
                prefabToSpawn = _enemy1;
                spawnTransform = GetRandomSpawn(_horizontalSpawns);
                break;
            case 1:
                prefabToSpawn = _enemy2;
                spawnTransform = GetRandomSpawn(_verticalSpawns);
                break;
            case 2:
                prefabToSpawn = _enemy3;
                spawnTransform = GetRandomSpawn(_diagonalSpawns);
                break;
        }
        Instantiate(prefabToSpawn, spawnTransform.position, Quaternion.identity);
    }
    private Transform GetRandomSpawn(Transform spawnParent)
    {
        int count = spawnParent.childCount;
        int r = Random.Range(0, count);
        return spawnParent.GetChild(r);
    }
}
