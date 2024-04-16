using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Pistol : BaseWeapon
{
    SpriteRenderer sr;
    BoxCollider2D coll;
    [SerializeField] GameObject player = null;

    [SerializeField] string gunName = "Пекаль"; 
    [SerializeField] int ammo = 30;
    [HideInInspector] public Transform firePoint;

    bool isEquiped = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(isEquiped && Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

        if(isEquiped && Input.GetButtonDown("Fire2"))
        {
            GunFly();
        }
    }
    public override void Attack()
    {
        if (ammo > 0)
        {
            GameObject bullet = GameObject.Instantiate(Resources.Load("Objects/Gun/Bullet") as GameObject, firePoint.position, player.transform.rotation);
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody.AddForce(player.transform.up * 18, ForceMode2D.Impulse);

            ammo--;
        }
        if (ammo == 0)
            Debug.Log($"В {gunName} больше нет патрон");
    }

    public void GunFly()
    {
        Fly(sr, coll, firePoint);
        isEquiped = false;
        player = null;
    }

    public void PickUpGun(GameObject dupaPlayer)
    {
        player = dupaPlayer;
        PickUp(player, sr, coll, gameObject);
        isEquiped = true;
    }

}
