using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class RangedWeapon : Weapon
{
    [SerializeField] protected float ammo, spray, attackCooldown, bulletSpeed, bulletsPerShoot;

    [SerializeField] protected GameObject _bullet;


    [Command]
    public override void CmdAttack(Vector2 shootDirection)
    {
        if (timeBetweenAttacks <= 0 && ammo > 0)
        {
            ammo--;
            timeBetweenAttacks = attackCooldown;
            for (int i = 0; i < bulletsPerShoot; i++)
            {
                float shift = Random.Range(-spray, spray);
                GameObject bullet = Instantiate(_bullet, firePoint.position, Quaternion.LookRotation(Vector3.forward, new Vector2(shootDirection.x + shift, shootDirection.y + shift)));
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootDirection.x + shift, shootDirection.y + shift).normalized * bulletSpeed;
            }
        }
    }
}