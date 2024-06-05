using System.Collections;
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
    [SerializeField] int[] enemiesCount;

    [SerializeField]List<object> objectsToDestroy = new List<object>();

    void Start()
    {
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        CmdSpawnObjects();
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnObjects()
    {
        CreateSpawners();
        CreateEnemyManager();
        CreateTeleporter();
    }

    void CreateSpawners()
    {
        for (int i = 0; i < enemySpawnerPositions.Length; i++)
        {
            GameObject spawnerClone = Instantiate(enemySpawner.gameObject, enemySpawnerPositions[i].position, Quaternion.identity);

            NetworkServer.Spawn(spawnerClone.gameObject);

            //spawnerClone.GetComponent<EnemySpawner>().SetParameters(enemiesCount[i], paths[i]);
            spawnerClone.GetComponent<EnemySpawner>()._enemiesCount = enemiesCount[i];
            spawnerClone.GetComponent<EnemySpawner>()._path = paths[i];
            objectsToDestroy.Add(spawnerClone);
        }
    }

    void CreateEnemyManager()
    {
        GameObject enManager = Instantiate(enemyManager.gameObject, transform.position, Quaternion.identity);
        NetworkServer.Spawn(enManager.gameObject);
        objectsToDestroy.Add(enManager);
    }

    void CreateTeleporter()
    {
        GameObject portal = Instantiate(teleporter.gameObject, teleporterSpawnPos.position, Quaternion.identity);
        NetworkServer.Spawn(portal);
        objectsToDestroy.Add(portal);
    }


    public void DestroyAllObjectsOnMap()
    {
        foreach (object obj in objectsToDestroy)
        {
            Destroy(obj as GameObject);
        }
        foreach (Weapon weapon in EnemyManager.Instance.enemiesWeapons)
        {
            NetworkServer.Destroy(weapon.gameObject);
        }
    }
}
