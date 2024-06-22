using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public abstract class Weapon : NetworkBehaviour
{
    public string weaponName;

    [SerializeField] protected LayerMask enemyLayer;

    Rigidbody2D rb;

    [SyncVar]
    float flyingTime;

    [SyncVar]
    protected float timeBetweenAttacks;

    [SyncVar]
    public bool pickUpAvailable;

    [SyncVar]
    bool canAttack;
    [SyncVar]
    public bool isInEnemiesHands;

    [SyncVar]
    [SerializeField] public Vector2 lookDirection;

    [SerializeField] protected Transform attackPoint;

    [SerializeField]
    protected AudioClip attackSound;
    [SerializeField]
    protected AudioClip pickUpSound;
    [SerializeField]
    protected AudioClip dropSound;


    protected AudioPlayer audioPlayer;

    void Start()
    {
        audioPlayer = gameObject.AddComponent<AudioPlayer>();
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
            GetComponent<SpriteRenderer>().enabled = false;
            this.transform.SetParent(attachmentPoint);

            this.gameObject.transform.localPosition = attackPointPosition;
            transform.rotation = attachmentPoint.rotation;
            this.pickUpAvailable = false;
            canAttack = true;
        }
    }

    void PlayPickUpSound()
    {
        audioPlayer.PlaySound(pickUpSound);
    }

    [Command(requiresAuthority = false)]
    public void CmdPlayPickUpSound()
    {
        PlayPickUpSound();
    }

    [ClientRpc]
    public void RpcPlayPickUpSound()
    {
        PlayPickUpSound();
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
        canAttack = false;
        this.gameObject.transform.parent = null;
        flyingTime = localFlyingTime;
        isInEnemiesHands = false;

        audioPlayer.PlaySound(dropSound);
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

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.TryGetComponent<Player>(out _)) return;

        if (coll.gameObject.TryGetComponent<IHitable>(out IHitable target) && flyingTime > 0)
        {
            target.Hit();
            pickUpAvailable = true;
            flyingTime = 0;
            rb.velocity = Vector2.zero;
        }
    }

    //-------------ATTACK---------------
    public virtual void Attack(Vector2 shootDirection)
    {
        if(!canAttack) return;
    }
}