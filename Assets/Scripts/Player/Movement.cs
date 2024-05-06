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

    [SyncVar]
    Vector2 _mousePosition;
    Vector2 lookDirection;

    [SyncVar]
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
        _mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Fire2"))
        {
            CmdDropGun();
            Debug.Log(_mousePosition);
            CmdTryPickUpGun(_mousePosition);
        }

        if (Input.GetButton("Fire1"))
        {
            if (gun != null) CmdAttack(_mousePosition);
        }

        if (gun != null)
        {
            gun.lookDirection = lookDirection;
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        lookDirection = _mousePosition - _rigidbody.position;
        _rigidbody.velocity = new Vector2(_movement.x, _movement.y).normalized * _moveSpeed;
        _rigidbody.rotation = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
    }

    [Command(requiresAuthority = false)]
    void CmdTryPickUpGun(Vector2 mousePos)
    {
        Debug.Log("CmdTryPickUpGun");
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, gunMask);
        if (hit.collider != null && Vector2.Distance(transform.position, hit.collider.gameObject.transform.position) <= .8f)
        {
            Debug.Log("Zaszel wpopu");
            gun = hit.collider.GetComponent<Weapon>();
            if (!gun.pickUpAvailable) return;
            gun.firePoint = transform.GetChild(0);
            gun.CmdPickUp(transform);
            //CmdPickUpGun(hit);
        }
    }

    [Command(requiresAuthority = false)]
    void CmdDropGun()
    {
        Debug.Log("CmdDropGun");
        if (gun == null) return;
        gun.CmdDrop();
        gun = null;
    }

    [Command(requiresAuthority = false)]
    void CmdAttack(Vector2 mousePos)
    {
        if (gun != null)
        {
            Debug.Log("attack");
            //if (gun == null) return;
            Debug.Log(gun);
            gun.CmdAttack(mousePos - _rigidbody.position);
        }
    }
}