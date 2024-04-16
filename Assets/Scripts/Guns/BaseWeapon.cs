using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public abstract void Attack();

    public void PickUp(GameObject player, SpriteRenderer sr, BoxCollider2D collider, GameObject gun)
    {
        sr.enabled = false;
        collider.enabled = false;

        Debug.Log("Поднял с пола как опущенный");
    }

    public void Fly(SpriteRenderer sr, BoxCollider2D collider, Transform throwPoint)
    {
        sr.enabled = true;
        collider.enabled = true;
        gameObject.transform.position = throwPoint.position;
        gameObject.AddComponent<FlyingGun>();

        Debug.Log("Кинул наверное");
    }

}
