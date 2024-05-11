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
        if (collision.gameObject.TryGetComponent<IHitable>(out IHitable target))
        {
            if (collision.gameObject.TryGetComponent<Player>(out _))
            {
                return;
            }
            DestroyBullet();
            target.Hit();
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
