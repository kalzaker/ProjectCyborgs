using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] Enemy[] enemies;
    [SerializeField] public int _enemiesCount;

    [SerializeField] GameObject _playerPosPoint;

    public readonly SyncList<Vector2> _points = new SyncList<Vector2>();

    public override void OnStartClient()
    {
        base.OnStartClient();
        _playerPosPoint = GameObject.Find("DEBUG.PlayerLastSeenPos");

        // Ensure points are populated before spawning enemies
        if (_points.Count > 0)
        {
            Vector2[] arrayPoints = new Vector2[_points.Count];
            for(int i = 0; i < _points.Count; i++)
            {
                arrayPoints[i] = _points[i];
            }
            CmdRequestSpawnEnemies(arrayPoints);
        }
    }

    [Command(requiresAuthority = false)]
    void CmdRequestSpawnEnemies(Vector2[] points)
    {
        _points.Clear();
        foreach (var point in points)
        {
            _points.Add(point);
        }

        // Now actually spawn enemies on the server
        SpawnEnemies(points);

        // Notify clients
        RpcSpawnEnemies(points);
    }

    [ClientRpc]
    void RpcSpawnEnemies(Vector2[] points)
    {
        if (!isServer)
        {
            SpawnEnemies(points);
        }
    }

    void SpawnEnemies(Vector2[] points)
    {
        for (int i = 0; i < _enemiesCount; i++)
        {
            GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)].gameObject, transform.position, Quaternion.identity);
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            MapManager.Instance.objectsToDestroy.Add(enemy);
            if (enemyComponent != null)
            {
                enemyComponent.debugPlayerPosPoint = _playerPosPoint;
                enemyComponent.path = new Path { waypoints = new List<Transform>() };

                foreach (var point in points)
                {
                    GameObject tempPoint = new GameObject("Waypoint");
                    tempPoint.transform.position = point;
                    enemyComponent.path.waypoints.Add(tempPoint.transform);
                    MapManager.Instance.objectsToDestroy.Add(tempPoint);
                }

                // Only the server should call NetworkServer.Spawn
                if (isServer)
                {
                    NetworkServer.Spawn(enemy);
                }
            }
            else
            {
                Debug.LogError("Enemy component not found on instantiated object");
            }
        }
    }
}