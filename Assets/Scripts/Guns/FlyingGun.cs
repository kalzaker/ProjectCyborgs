using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGun : MonoBehaviour
{
    float flyingTime = 2f;
    Vector3 direction;
    Rigidbody2D rb;

    private void Start()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        direction.x = mousePos.x - transform.position.x;
        direction.y = mousePos.y - transform.position.y;
        direction.z = 0;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction * 40);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    private void Update()
    {

        transform.Rotate(0f, 0f, Mathf.Lerp(400f, 0f, Time.deltaTime) * Time.deltaTime);
        flyingTime -= Time.deltaTime;
        if(flyingTime <= 0)
        {
            rb.bodyType = RigidbodyType2D.Static;
            Destroy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Hit enemy
        }
        else if (!collision.CompareTag("Player"))
        {
            Debug.Log("че");
            rb.bodyType = RigidbodyType2D.Static;
            Destroy(this);
        }
    }
}
