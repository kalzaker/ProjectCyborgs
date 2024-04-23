using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class Movement : NetworkBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] LayerMask gunMask;

    Camera cam;

    Rigidbody2D _rigidbody;

    Vector2 _movement;
    Vector2 _mousePosition;

    [SerializeField] Weapon gun;

    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            cam = Camera.main;
        }       
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        _mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Fire2"))
        {
            DropGun();
            TryPickUpGun();
        }

        if(Input.GetButton("Fire1") && gun != null)
        {
            gun.CmdAttack(_mousePosition - _rigidbody.position);
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        _rigidbody.velocity = new Vector2(_movement.x, _movement.y).normalized * _moveSpeed;

        Vector2 lookDirection = _mousePosition - _rigidbody.position;
        _rigidbody.rotation = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;


        
    }

    void TryPickUpGun()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, gunMask);
        if (hit.collider != null && Vector2.Distance(transform.position, hit.collider.gameObject.transform.position) <= .8f)
        {
            gun = hit.collider.GetComponent<Weapon>();            
            gun.firePoint = transform.GetChild(0);
            gun.PickUp(transform);
        }
    }

    void DropGun()
    {
        if (gun == null) return;
        gun.Drop();
        gun = null;
    }
}
