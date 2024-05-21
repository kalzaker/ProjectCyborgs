using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour, IHitable
{
    //float timeTillDeath;
    [SyncVar]
    public bool alive;

    void Start()
    {
        alive = true;
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
        alive = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Die();
    }

    void Die()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //GetComponent<Rigidbody2D>().enabled = false;
        Debug.Log("PIZDA");

        //Time.timeScale = 0;
    }
}