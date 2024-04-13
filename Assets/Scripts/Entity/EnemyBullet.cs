using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private void Start()
    {
        Invoke("DestroyBullet", 7f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Player")) 
        {
            Debug.Log("Попал по жопi");
        }
        DestroyBullet();
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
