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

    [SerializeField]
    EnemySpawnerParameters[] spawnerParametersArray;

    [SerializeField] Transform teleporterSpawnPos;

    [SerializeField] List<Path> paths;


    void Start()
    {
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        spawnerParametersArray = FindObjectsOfType<EnemySpawnerParameters>();

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
        for (int i = 0; i < spawnerParametersArray.Length; i++)
        {
            CreateSpawner(spawnerParametersArray[i]);
        }
    }

    void CreateSpawner(EnemySpawnerParameters parameters)
    {
        parameters = parameters.GetComponent<EnemySpawnerParameters>();
        if (!isServer) return;
        GameObject spawnerClone = Instantiate(enemySpawner.gameObject, GetSpawnerPositionFromSpawnerParameters(parameters).position, Quaternion.identity);

        spawnerClone.GetComponent<EnemySpawner>()._enemiesCount = GetEnemiesCountFromSpawnerParameters(parameters);

        Vector2[] points = GetWaypointsPositions(GetPathFromSpawnerParameters(parameters));
        for (int i = 0; i < points.Length; i++)
        {
            spawnerClone.GetComponent<EnemySpawner>()._points.Add(points[i]);
        }

        Debug.Log(spawnerClone);
        MapManager.Instance.objectsToDestroy.Add(spawnerClone as GameObject);
        Debug.Log(MapManager.Instance.objectsToDestroy);

        NetworkServer.Spawn(spawnerClone);

    }

    void CreateEnemyManager()
    {
        if (!isServer) return;
        GameObject enManager = Instantiate(enemyManager.gameObject, transform.position, Quaternion.identity);
        NetworkServer.Spawn(enManager);
        MapManager.Instance.objectsToDestroy.Add(enManager);
    }

    void CreateTeleporter()
    {
        if (!isServer) return;
        GameObject portal = Instantiate(teleporter.gameObject, teleporterSpawnPos.position, Quaternion.identity);
        NetworkServer.Spawn(portal);
        MapManager.Instance.objectsToDestroy.Add(portal);
    }

    

    Path GetPathFromSpawnerParameters(EnemySpawnerParameters spawnerParams)
    {
        Path tempPath = new Path();
        for(int i = 0; i < spawnerParams.GetComponent<EnemySpawnerParameters>().pathParentObject.childCount; i++)
        {
            tempPath.waypoints.Add(spawnerParams.GetComponent<EnemySpawnerParameters>().pathParentObject.GetChild(i));
        }
        

        return tempPath;
    }

    Transform GetSpawnerPositionFromSpawnerParameters(EnemySpawnerParameters spawnerParams)
    {
        Transform transform = spawnerParams.GetComponent<EnemySpawnerParameters>().transform; 
        
        return transform;
    }

    int GetEnemiesCountFromSpawnerParameters(EnemySpawnerParameters spawnerParams)
    {
        int count = spawnerParams.GetComponent<EnemySpawnerParameters>().enemiesCount;

        return count;
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