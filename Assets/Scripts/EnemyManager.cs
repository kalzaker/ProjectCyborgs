using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyManager : NetworkBehaviour
{
    public static EnemyManager Instance;

    public event Action AllEnemiesDefeated;

    private List<Enemy> enemies = new List<Enemy>();

    public List<Weapon> enemiesWeapons = new List<Weapon>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Enemy[] initialEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in initialEnemies)
        {
            RegisterEnemy(enemy);
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            enemy.OnEnemyKilled += HandleEnemyKilled;
        }
    }

    private void HandleEnemyKilled(Enemy enemy)
    {
        enemies.Remove(enemy);
        enemy.OnEnemyKilled -= HandleEnemyKilled;

        if (enemies.Count == 0)
        {
            AllEnemiesDefeated?.Invoke();
        }
    }
}