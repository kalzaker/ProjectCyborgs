using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public abstract class Weapon : NetworkBehaviour
{
    [SerializeField] protected LayerMask enemyLayer;

    Rigidbody2D rb;

    [SyncVar]
    float flyingTime;

    protected float timeBetweenAttacks;

    [SyncVar]
    public bool pickUpAvailable;

    [SyncVar]
    bool canAttack;
    public bool isInEnemiesHands;

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
            Debug.Log(timeBetweenAttacks);
        }
        if(flyingTime > 0)
        {
            Debug.Log(flyingTime);
            Fly();
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSetLookDirection(Vector2 lookDirection)
    {
        SetLookDirection(lookDirection);
    }

    [ClientRpc]
    public void RpcSetLookDirection(Vector2 lookDirection)
    {
        SetLookDirection(lookDirection);
    }

    public void SetLookDirection(Vector2 lookDirection)
    {
        this.lookDirection = lookDirection;
    }

    //------------PICK UP-------------
    [Command(requiresAuthority = false)]
    public void CmdPickUp(Transform attachmentPoint, Vector2 attackPointPosition)
    {
        PickUp(attachmentPoint, attackPointPosition);
    }

    [ClientRpc]
    public void RpcPickUp(Transform attachmentPoint, Vector2 attackPointPosition)
    {
        PickUp(attachmentPoint,attackPointPosition);
    }

    public void PickUp(Transform attachmentPoint, Vector2 attackPointPosition)
    {
        if (pickUpAvailable)
        {
            Debug.Log("PickUp");
            GetComponent<SpriteRenderer>().enabled = false;
            this.transform.SetParent(attachmentPoint);

            this.gameObject.transform.localPosition = attackPointPosition;
            transform.rotation = attachmentPoint.rotation;
            this.pickUpAvailable = false;
            Debug.Log(pickUpAvailable);
            canAttack = true;
            Debug.Log(attachmentPoint);
        }
    }


    //----------DROP----------
    [Command(requiresAuthority = false)]
    public void CmdDrop(float localFlyingTime)
    {
        Drop(localFlyingTime);
    }

    [ClientRpc]
    public void RpcDrop(float localFlyingTime)
    {
        Drop(localFlyingTime);
    }

    public void Drop(float localFlyingTime)
    {
        GetComponent<SpriteRenderer>().enabled = true;
        Debug.Log("Drop");
        canAttack = false;
        this.gameObject.transform.parent = null;
        flyingTime = localFlyingTime;
        isInEnemiesHands = false;
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

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.TryGetComponent<Player>(out _)) return;

        if (coll.TryGetComponent<IHitable>(out IHitable target) && flyingTime > 0)
        {
            target.Hit();
            pickUpAvailable = true;
            flyingTime = 0;
            rb.velocity = Vector2.zero;
        }
    }

    //-------------ATTACK---------------
    public void Attack(Vector2 shootDirection)
    {
        CmdAttack(shootDirection);
    }

    [Command(requiresAuthority = false)]
    public virtual void CmdAttack(Vector2 shootDirection)
    {
        if (!canAttack) return;
    }
}