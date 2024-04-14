using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] string gunName = "FIST";
    [SerializeField] bool isRanged = false;
    [SerializeField] int ammo = 0;
    [SerializeField] Transform firePoint;
    float fireRate = 1f;
    float nextFireTime = 0f;
    float nextAttackTime = 0f;
    [SerializeField] bool haveGun = false;
    GameObject gun;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (isRanged && haveGun)
            {
                Fire();
            }
            else
            {
                Attack();
            }
        }

        if (Input.GetButtonDown("Fire2") && haveGun)
        {
            ThrowGun();            
        }
    }
    void Fire() 
    {
        if (ammo > 0 && Time.time > nextFireTime)
        {
            GameObject bullet = GameObject.Instantiate(Resources.Load("Objects/Gun/Bullet") as GameObject, firePoint.position, transform.rotation);
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody.AddForce(transform.up * 18, ForceMode2D.Impulse);

            ammo--;
            nextFireTime = Time.time + 1f / fireRate;
        }
        if (ammo == 0)
            Debug.Log($"В {gunName} нет больше патрон");
        
    }

    void Attack()
    {
        if(Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / fireRate;
            Debug.Log("Махнул плотно рукой");
        }
            
    }

    void ThrowGun()
    {
        haveGun = false;
        gun.GetComponent<PickUpGun>().Fly(ammo);

        gunName = "FIST";
        isRanged = false;
        fireRate = 1f;
        ammo = 0;

        gun.SetActive(true);
        gun.transform.SetParent(null);

        gun.transform.position = firePoint.position;
        gun.AddComponent<FlyingGun>();

        

        Debug.Log("кинул нах");
    }

    public void GetGun(string name, bool IsRanged, int Ammo, float FireRate, GameObject Gun)
    {
        haveGun = true;

        gunName = name;
        isRanged = IsRanged;
        ammo = Ammo;
        fireRate = FireRate;
        gun = Gun;

        gun.SetActive(false);
        gun.transform.SetParent(transform);
        gun.transform.localPosition = Vector3.zero;
        

        Debug.Log($"Подобрал {gunName}");
    }
}
