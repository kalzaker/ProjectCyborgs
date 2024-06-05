using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemySpawner : NetworkBehaviour
{
    
    [SerializeField] Enemy[] enemies;
    [SerializeField] public int _enemiesCount;

    [SerializeField] public Path _path;

    [SerializeField] GameObject playerPosPoint;

    void Start()
    {
        if (!isServer) return;
        CmdSpawnEnemies();
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnEnemies()
    {
        for (int i = 0; i < _enemiesCount; i++)
        {
            GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length - 1)].gameObject, transform.position, Quaternion.identity);
            NetworkServer.Spawn(enemy);

            enemy.GetComponent<Enemy>().path = _path;
            enemy.GetComponent<Enemy>().debugPlayerPosPoint = playerPosPoint;
        }
    }

    public void SetParameters(int enemiesCount, Path path)
    {
        _enemiesCount = enemiesCount;
        _path = path; 
    }
}
