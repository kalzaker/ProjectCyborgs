using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FlyingGun : NetworkBehaviour
{
    float flyingTime = 1f;
    [SerializeField]Vector3 direction;
    Rigidbody2D rb;
    [SerializeField] float velocity;

    private void Start()
    {
        RpcFly();
    }

    private void Update()
    {
        RpcRotate();
    }

    [ClientRpc]
    void RpcFly()
    {
        direction = this.gameObject.GetComponent<Weapon>().lookDirection;

        direction = direction.normalized;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * 40);
        GetComponent<Weapon>().pickUpAvailable = false;
    }

    [ClientRpc]
    void RpcRotate()
    {
        transform.Rotate(0f, 0f, Mathf.Lerp(400f, 0f, Time.deltaTime) * Time.deltaTime);
        flyingTime -= Time.deltaTime;
        velocity = rb.velocity.x;
        if (flyingTime <= 0)
        {
            //rb.bodyType = RigidbodyType2D.Static;
            DestroyThisComponent();
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHitable>(out IHitable target))
        {
            target.Hit();
            DestroyThisComponent();
        }
        
    }

    void SetPickAvailableTrue()
    {
        GetComponent<Weapon>().pickUpAvailable = true;
    }

    void DestroyThisComponent()
    {
        Destroy(this);
        SetPickAvailableTrue();
    }
}
