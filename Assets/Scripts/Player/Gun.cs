using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _firePoint;

    [SerializeField] float _bulletSpeed = 20f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, transform.rotation);
        Rigidbody2D _bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        _bulletRigidbody.AddForce(transform.up * _bulletSpeed, ForceMode2D.Impulse);
    }
}
