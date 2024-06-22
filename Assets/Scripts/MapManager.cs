using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MapManager : NetworkBehaviour
{
    public static MapManager Instance;

    [SyncVar] bool mapAlreadyExists;

    Player[] players;


    [SerializeField]
    private GameObject[] mapPrefabs;
    [SyncVar(hook = nameof(OnRandomMapNumberChanged))]
    int randomMapNumber;

    GameObject mapInstance;

    [SerializeField] Teleporter lobbyPortal;

    [SyncVar]
    public bool needToSpawnMap = true;

    [SerializeField] Transform lobbySpawnPoint;

    [SyncVar]
    [SerializeField] public List<GameObject> objectsToDestroy = new List<GameObject>();

    [SerializeField] Weapon[] weapons;

    void Awake()
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

    public override void OnStartClient()
    {
        base.OnStartClient();

        RespawnStartingWeapons();
    }

    public void SpawnRandomMap()
    {
        if (!isServer) return;


        randomMapNumber = UnityEngine.Random.Range(0, mapPrefabs.Length);

        GameObject selectedMapPrefab = mapPrefabs[randomMapNumber];

        mapInstance = Instantiate(selectedMapPrefab, transform.position, Quaternion.identity);

        NetworkServer.Spawn(mapInstance);

        needToSpawnMap = false;
    }

    void OnRandomMapNumberChanged(int oldValue, int newValue)
    {
        Debug.Log($"Random map number changed from {oldValue} to {newValue}");
    }

    public void TeleportPlayer(Teleporter teleporter)
    {
        RpcTeleportPlayer(GetSpawnPosition(teleporter, mapInstance));
    }

    private Vector3 GetSpawnPosition(Teleporter teleporter, GameObject map)
    {
        if (!teleporter.isLobbyPortal)
        {
            needToSpawnMap = true;
            RespawnStartingWeapons();
            return lobbySpawnPoint.position;
        }

        return map.GetComponent<Map>().spawnPoint.position;
    }

    private Vector3 GetSpawnPosition(GameObject map)
    {
        return map.GetComponent<Map>().spawnPoint.position;
    }

    [ClientRpc]
    private void RpcTeleportPlayer(Vector3 position)
    {
        foreach (Player player in FindAllPlayers())
        {
            player.transform.position = position;
        }
    }

    [Command(requiresAuthority = false)]
    void CmdTeleportPlayer(Vector3 position)
    {
        RpcTeleportPlayer(position);
    }

    [ClientRpc]
    void RpcRespawnStartingWeapons()
    {
        RespawnStartingWeapons();
    }

    [Command(requiresAuthority = false)]
    void CmdRespawnStartingWeapons()
    {
        RespawnStartingWeapons();
    }

    void RespawnStartingWeapons()
    {
        if (!isServer) return;
        for (int i = 0; i < 2; i++)
        {
            GameObject startingWeapon = Instantiate(weapons[Random.Range(0, weapons.Length)].gameObject, new Vector2(Random.Range(-32, -21), Random.Range(2, 9)), Quaternion.identity);
            NetworkServer.Spawn(startingWeapon);
            objectsToDestroy.Add(startingWeapon);
        }
    }

    Player[] FindAllPlayers()
    {
        return FindObjectsOfType<Player>();
    }

    public void DestroyMap()
    {
        DestroyAllObjectsOnMap();
        NetworkServer.Destroy(mapInstance);
    }

    [Command(requiresAuthority = false)]
    void CmdDestroyMap()
    {
        RpcDestroyMap();
    }

    [ClientRpc]
    void RpcDestroyMap()
    {
        DestroyAllObjectsOnMap();
        NetworkServer.Destroy(mapInstance);
    }


    public void DestroyAllObjectsOnMap()
    {
        if (!isServer) return;
        foreach (Weapon weapon in EnemyManager.Instance.enemiesWeapons)
        {
            if (weapon != null)
                NetworkServer.Destroy(weapon.gameObject);
        }
        RpcDestroy();
    }

    [ClientRpc]
    void RpcDestroy()
    {
        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
            {

                Destroy(obj);
            }
        }
    }

    [Command(requiresAuthority = false)] 
    void CmdDestroy()
    {
        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }

    public void RestartGame()
    {
        DestroyAllObjectsOnMap();
        RpcRespawnStartingWeapons();
        if (mapInstance == null) return;
        TeleportPlayer(mapInstance.GetComponent<Map>().teleporter);
        DestroyMap();
    }
}