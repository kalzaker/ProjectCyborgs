using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Teleporter : NetworkBehaviour
{
    [SerializeField] Transform targetPosition;

    [SerializeField] Map map;

    [SerializeField] public bool isLobbyPortal;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isLobbyPortal) return;
        EnemyManager.Instance.AllEnemiesDefeated += ActivatePortal;
        gameObject.SetActive(false);
    }
    
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!isServer) return;
        if(coll.TryGetComponent<Player>(out Player player))
        {
            NetworkIdentity playerNetId = player.GetComponent<NetworkIdentity>();
            if (playerNetId != null && MapManager.Instance.needToSpawnMap)
            {
                MapManager.Instance.SpawnRandomMap();
            }
            MapManager.Instance.TeleportPlayer(this.GetComponent<Teleporter>());
            if(!isLobbyPortal)
            {
                MapManager.Instance.RestartGame();
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
        gameObject.SetActive(true);
        // Ваш код для активации портала
    }
}