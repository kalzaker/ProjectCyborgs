using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public abstract class Weapon : NetworkBehaviour
{
    Rigidbody2D rb;

    [SyncVar]
    float flyingTime;

    protected float timeBetweenAttacks;

    [SyncVar]
    public bool pickUpAvailable;

    [SyncVar]
    bool canAttack;

    [SyncVar]
    [SerializeField] public Vector2 lookDirection;

    [SerializeField] protected Transform attackPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pickUpAvailable = true;
    }

    void Update()
    {

        if (pickUpAvailable) return;
        if (timeBetweenAttacks >= 0)
        {
            timeBetweenAttacks -= Time.deltaTime;
        }
        if(flyingTime > 0)
        {
            Fly();
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdPickUp(Transform attachmentPoint, Vector2 attackPointPosition)
    {
        RpcPickUp(attachmentPoint, attackPointPosition);
    }

    [ClientRpc]
    void RpcPickUp(Transform attachmentPoint, Vector2 attackPointPosition)
    {
        Debug.Log("PickUp");
        if (!pickUpAvailable) return;
        this.transform.SetParent(attachmentPoint);
        this.gameObject.transform.localPosition = attackPointPosition;
        //attackPoint.localPosition = attackPointPosition;
        transform.rotation = attachmentPoint.rotation;
        pickUpAvailable = false;
        canAttack = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdDrop()
    {
        RpcDrop();
    }

    [ClientRpc]
    void RpcDrop()
    {
        Debug.Log("Drop");
        canAttack = false;
        this.gameObject.transform.parent = null;
        flyingTime = 1.2f;
    }

    [Command(requiresAuthority = false)]
    public virtual void CmdAttack(Vector2 shootDirection)
    {
        if (!canAttack) return;
    }

    [Command(requiresAuthority = false)]
    void CmdFly()
    {
        Fly();
    }

    void Fly()
    {
        rb.velocity = lookDirection.normalized * 10 * flyingTime * flyingTime;
        pickUpAvailable = false;

        transform.Rotate(0f, 0f, Mathf.Lerp(600f * flyingTime, 0f, Time.deltaTime) * Time.deltaTime);
        flyingTime -= Time.deltaTime;
        if (flyingTime <= 0)
        {
            rb.velocity = Vector2.zero;
            pickUpAvailable = true;
            return;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSetLookDirection(Vector2 lookDirection)
    {
        RpcSetLookDirection(lookDirection);
    }

    [ClientRpc]
    void RpcSetLookDirection(Vector2 lookDirection)
    {
        this.lookDirection = lookDirection;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.TryGetComponent<IHitable>(out IHitable target) && flyingTime > 0)
        {
            target.Hit();
            pickUpAvailable = true;
            flyingTime = 0;
            rb.velocity = Vector2.zero;
        }
    }
}