using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponText : MonoBehaviour
{
    void Start()
    {
        EventManager.weaponPickedUp.AddListener(OnWeaponPicked);
        EventManager.weaponDropped.AddListener(OnWeaponDropped);
    }

    void OnWeaponPicked(Weapon weapon)
    {
        string text = weapon.weaponName;
        

        gameObject.GetComponent<TextMeshProUGUI>().text = "weapon: " + text;
    }

    void OnWeaponDropped()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = "weapon: none";
    }
}