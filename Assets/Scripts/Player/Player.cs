using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour, IHitable
{

    [SyncVar]
    public bool alive;

    [SyncVar]
    public bool canMove;

    void Start()
    {
        alive = true;
        canMove = true;
    }

    void Update()
    {
        
    }

    
    public void Hit()
    {
        CmdHit();
    }

    [Command(requiresAuthority = false)]
    void CmdHit()
    {
        RpcDie();
    }

    [ClientRpc]
    void RpcDie()
    {
        Die();
    }

    void Die()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        alive = false;
        canMove= false;

        EventManager.playerDied.Invoke();
    }
}