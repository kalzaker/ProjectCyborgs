using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGun : MonoBehaviour
{
    float flyingTime = 1f;
    Vector3 direction;
    Rigidbody2D rb;

    private void Start()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        direction.x = mousePos.x - transform.position.x;
        direction.y = mousePos.y - transform.position.y;
        direction.z = 0;
        direction = direction.normalized * 3;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction * 40);
        GetComponent<Weapon>().pickUpAvailable = false;
        Invoke("SetPickUpAvailableTrue", 0.3f);
    }

    private void Update()
    {

        transform.Rotate(0f, 0f, Mathf.Lerp(400f, 0f, Time.deltaTime) * Time.deltaTime);
        flyingTime -= Time.deltaTime;
        if(flyingTime <= 0)
        {
            rb.bodyType = RigidbodyType2D.Static;
            DestroyThisComponent();
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHitable>(out IHitable target))
        {
            target.Hit();
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
