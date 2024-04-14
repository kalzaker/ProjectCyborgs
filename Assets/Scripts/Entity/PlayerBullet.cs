using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    void Start()
    {
        Invoke("DestroyBullet", 7f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
            DestroyBullet();
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}