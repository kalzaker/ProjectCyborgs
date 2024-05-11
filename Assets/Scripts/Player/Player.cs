using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour, IHitable
{
    float timeTillDeath;
    [SyncVar]
    public bool alive;

    void Start()
    {
        alive = true;
        timeTillDeath = 10f;
    }

    void Update()
    {
        if(alive) return;
        if(!alive)
        {
            timeTillDeath -= Time.deltaTime;
        }

        if (timeTillDeath <= 0)
            Die();
    }

    
    public void Hit()
    {
        CmdHit();
    }

    [Command(requiresAuthority = false)]
    void CmdHit()
    {
        alive = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void Die()
    {
        Debug.Log("PIZDA");
    }
}
