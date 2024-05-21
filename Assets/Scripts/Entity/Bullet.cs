using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isEnemyBullet;

    void Start()
    {
        Invoke("DestroyBullet", 7f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IHitable>(out IHitable target))
        {
            if (collision.gameObject.TryGetComponent<Enemy>(out _) && isEnemyBullet)
            {
                return;
            }
            if (collision.gameObject.TryGetComponent<Player>(out _) && !isEnemyBullet)
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
