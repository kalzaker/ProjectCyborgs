using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour, IHitable
{
    //float timeTillDeath;
    [SyncVar]
    public bool alive;
    public bool canMove;

    void Start()
    {
        alive = true;
        canMove = true;
        //timeTillDeath = 10f;
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
        Die();
    }

    void Die()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        alive = false;
        canMove= false;
        //GetComponent<Rigidbody2D>().enabled = false;

        //Time.timeScale = 0;
    }
}