using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RangedWeapon : Weapon
{
    [SerializeField] AudioClip noAmmoSound;

    [SerializeField] protected float attackCooldown, spray, bulletSpeed, bulletsPerShoot;

    [SyncVar]
    public int ammo;

    [SyncVar]
    float shiftX;
    [SyncVar]
    float shiftY;

    [SerializeField]
    float soundRange;

    [SerializeField] protected GameObject _bullet;

    public override void Attack(Vector2 shootDirection)
    {
        if (timeBetweenAttacks > 0) return;

        if (ammo < 0)
        {
            audioPlayer.PlaySound(noAmmoSound);
            return;
        }

        ammo--;
        timeBetweenAttacks = attackCooldown;

        audioPlayer.PlaySound(attackSound);

        EventManager.shotFired.Invoke(ammo);
        CmdAttack(shootDirection);
    }

    [Command(requiresAuthority = false)]
    void CmdAttack(Vector2 shootDirection)
    {
        if (!isServer) return;

        //ammo--;
        //timeBetweenAttacks = attackCooldown;
        for (int i = 0; i < bulletsPerShoot; i++)
        {
            shiftX = Random.Range(-spray, spray);
            shiftY = Random.Range(-spray, spray);
            Vector2 bulletDirection = new Vector2(shootDirection.x + shiftX * shootDirection.x, shootDirection.y + shiftY * shootDirection.y);
            GameObject bullet = Instantiate(_bullet, attackPoint.position, Quaternion.LookRotation(Vector3.forward, bulletDirection));
            bullet.GetComponent<Bullet>().isEnemyBullet = isInEnemiesHands;
            if (isServer)
            {
                NetworkServer.Spawn(bullet);
            }
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection.normalized * bulletSpeed;
            MakeSound();
        }

        audioPlayer.PlaySound(attackSound);

        if (isInEnemiesHands) ammo++;
        if (ammo <= 0)
        {
            audioPlayer.PlaySound(noAmmoSound);
        }

    }

    [ClientRpc]
    void RpcAttack(Vector2 shootDirection)
    {
        if (!isServer) return;

        //ammo--;
        //timeBetweenAttacks = attackCooldown;

        for (int i = 0; i < bulletsPerShoot; i++)
        {
            shiftX = Random.Range(-spray, spray);
            shiftY = Random.Range(-spray, spray);
            Vector2 bulletDirection = new Vector2(shootDirection.x + shiftX*shootDirection.x, shootDirection.y + shiftY*shootDirection.y);
            GameObject bullet = Instantiate(_bullet, attackPoint.position, Quaternion.LookRotation(Vector3.forward, bulletDirection));
            bullet.GetComponent<Bullet>().isEnemyBullet = isInEnemiesHands;
            NetworkServer.Spawn(bullet);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection.normalized * bulletSpeed;
            MakeSound();
        }

        audioPlayer.PlaySound(attackSound);

        if (isInEnemiesHands) ammo++;
        if (ammo <= 0)
        {
            audioPlayer.PlaySound(noAmmoSound);
        }
    }

    void MakeSound()
    {


        audioPlayer.PlaySound(attackSound);

        //Enemy hear sounds
        if (isInEnemiesHands) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, soundRange, enemyLayer);

        foreach (Collider2D collider in colliders)
        {
            if(collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.LastPlayerKnownPos = gameObject.transform.position;
                enemy.GetComponent<StateMachine>().ChangeState(enemy.GetComponent<SearchState>());
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, soundRange);
    }
}