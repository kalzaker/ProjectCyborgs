using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Map : NetworkBehaviour
{
    public Transform spawnPoint;
    public Teleporter teleporter;
    public Enemy[] Enemies;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] EnemyManager enemyManager;

    [SerializeField] Transform teleporterSpawnPos;
    [SerializeField] Transform[] enemySpawnerPositions;

    [SerializeField] Path[] paths;

    [SyncVar]
    [SerializeField] int[] enemiesCount;

    [SerializeField] List<GameObject> objectsToDestroy = new List<GameObject>();

    void Start()
    {
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isServer)
        {
            CmdSpawnObjects();
        }
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnObjects()
    {
        RpcSpawnObjects();
    }

    [ClientRpc]
    void RpcSpawnObjects()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        if (!isServer) return;
        CreateSpawners();
        CreateEnemyManager();
        CreateTeleporter();
    }

    void CreateSpawners()
    {
        for (int i = 0; i < enemySpawnerPositions.Length; i++)
        {
            CreateSpawner(GetWaypointsPositions(paths[i]), i, enemiesCount[i]);
        }
    }

    void CreateSpawner(Vector2[] positions, int count, int enemiesCount)
    {
        if (!isServer) return;
        GameObject spawnerClone = Instantiate(enemySpawner.gameObject, enemySpawnerPositions[count].position, Quaternion.identity);

        spawnerClone.GetComponent<EnemySpawner>()._enemiesCount = enemiesCount;
        for (int i = 0; i < positions.Length; i++)
        {
            spawnerClone.GetComponent<EnemySpawner>()._points.Add(positions[i]);
        }

        NetworkServer.Spawn(spawnerClone);

        objectsToDestroy.Add(spawnerClone);
    }

    void CreateEnemyManager()
    {
        if (!isServer) return;
        GameObject enManager = Instantiate(enemyManager.gameObject, transform.position, Quaternion.identity);
        NetworkServer.Spawn(enManager);
        objectsToDestroy.Add(enManager);
    }

    void CreateTeleporter()
    {
        if (!isServer) return;
        GameObject portal = Instantiate(teleporter.gameObject, teleporterSpawnPos.position, Quaternion.identity);
        NetworkServer.Spawn(portal);
        objectsToDestroy.Add(portal);
    }

    public void DestroyAllObjectsOnMap()
    {
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
        foreach (Weapon weapon in EnemyManager.Instance.enemiesWeapons)
        {
            NetworkServer.Destroy(weapon.gameObject);
        }
    }

    Vector2[] GetWaypointsPositions(Path path)
    {
        Vector2[] result = new Vector2[path.waypoints.Count];
        for (int i = 0; i < path.waypoints.Count; i++)
        {
            result[i] = path.waypoints[i].position;
        }
        return result;
    }
}