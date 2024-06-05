using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Teleporter : NetworkBehaviour
{
    [SerializeField] Transform targetPosition;

    [SerializeField] MapManager _mapManager;

    [SerializeField] Map map;

    [SerializeField] public bool isLobbyPortal;

    
    
    void Start()
    {
        GameObject mapManager = GameObject.Find("MapManager");
        _mapManager = mapManager.GetComponent<MapManager>();
        if (isLobbyPortal) return;
        EnemyManager.Instance.AllEnemiesDefeated += ActivatePortal;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!isServer) return;
        if(coll.TryGetComponent<Player>(out Player player))
        {
            NetworkIdentity playerNetId = player.GetComponent<NetworkIdentity>();
            if (playerNetId != null && _mapManager.needToSpawnMap)
            {
                _mapManager.SpawnRandomMap();
            }
            _mapManager.TeleportPlayer(this.GetComponent<Teleporter>());
            if(!isLobbyPortal)
            {
                _mapManager.DestroyMap();
            }
        }
    }

    private void OnDestroy()
    {
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.AllEnemiesDefeated -= ActivatePortal;
        }
    }

    private void ActivatePortal()
    {
        // Логика для активации портала
        Debug.Log("All enemies defeated. Portal is now active!");
        // Ваш код для активации портала
    }
}