using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class PickUpGun : MonoBehaviour
{
    [SerializeField] string gunName;
    [SerializeField] int ammo;
    [SerializeField] float fireRate;
    [SerializeField] bool isRanged;

    GameObject player;
    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }

    void OnMouseOver()
    {
        if (player != null && Input.GetKey(KeyCode.E))
        {
            
        }
    }

    public void Fly(int Ammo)
    {
        ammo = Ammo;
    } 
}
