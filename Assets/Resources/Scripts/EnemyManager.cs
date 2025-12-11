using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
    public Enemy CurrentEnemy { get; private set; }

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

    void Start()
    {

    }

    void Update()
    {
        if(CurrentEnemy == null && Enemies.Count != 0)
        {
            CurrentEnemy = Enemies[0];
        }
    }

}
