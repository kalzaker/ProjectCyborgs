using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoText : MonoBehaviour
{
    void Start()
    {
        EventManager.shotFired.AddListener(OnShotFired);
        EventManager.weaponPickedUp.AddListener(OnWeaponPicked);
        EventManager.weaponDropped.AddListener(OnWeaponDropped);
    }

    void OnShotFired(int ammo)
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = ammo.ToString();
    }

    void OnWeaponPicked(Weapon weapon)
    {
        if(weapon.TryGetComponent<RangedWeapon>(out RangedWeapon gun))
        {
            gameObject.SetActive(true);
            gameObject.GetComponent<TextMeshProUGUI>().text = gun.ammo.ToString();
        }
    }

    void OnWeaponDropped()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = "";
    }
}
