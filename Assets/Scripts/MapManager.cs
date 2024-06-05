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
    GameObject mapInstance;

    [SyncVar]
    public bool needToSpawnMap = true;

    [SerializeField] Transform lobbySpawnPoint;

    public void SpawnRandomMap()
    {
        if (!isServer) return;

        int randomIndex = Random.Range(0, mapPrefabs.Length - 1);
        GameObject selectedMapPrefab = mapPrefabs[randomIndex];

        mapInstance = Instantiate(selectedMapPrefab, transform.position, Quaternion.identity);

        NetworkServer.Spawn(mapInstance);

        needToSpawnMap = false;
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
            return lobbySpawnPoint.position;
        }

        return map.transform.Find("SpawnPoint").position;
    }

    [ClientRpc]
    private void RpcTeleportPlayer(Vector3 position)
    {
        foreach (Player player in FindAllPlayers())
        {
            player.transform.position = position;
        }
    }

    Player[] FindAllPlayers()
    {
        return FindObjectsOfType<Player>();
    }

    public void DestroyMap()
    {
        mapInstance.GetComponent<Map>().DestroyAllObjectsOnMap();
        NetworkServer.Destroy(mapInstance);
    }
}