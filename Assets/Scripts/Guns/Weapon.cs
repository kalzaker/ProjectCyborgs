using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public abstract class Weapon : NetworkBehaviour
{
    protected float timeBetweenAttacks;

    public bool pickUpAvailable { get; set; }

    
    public Transform firePoint { get; set; }

    void Update()
    {
        if (timeBetweenAttacks >= 0)
        {
            timeBetweenAttacks -= Time.deltaTime;
        }
    }

    public void PickUp(Transform attachmentPoint)
    {
        if (!pickUpAvailable) return;
        this.GetComponent<Rigidbody2D>().transform.SetParent(attachmentPoint);
        this.gameObject.transform.localPosition = Vector2.zero;
    }

    public void Drop()
    {
        this.gameObject.transform.localPosition = Vector2.zero;
        this.gameObject.transform.parent = null;
        gameObject.AddComponent<FlyingGun>();
    }

    [Command]
    public virtual void CmdAttack(Vector2 shootDirection)
    {

    }
}
