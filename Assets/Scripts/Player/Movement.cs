using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class Movement : NetworkBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] LayerMask gunMask;
    [SerializeField] LayerMask usableObjectMask;

    Camera cam;

    Rigidbody2D _rigidbody;

    Vector2 _movement;

    [SyncVar]
    Vector2 _mousePosition;

    bool skipWeaponActions;

    Vector2 lookDirection;

    [SyncVar]
    [SerializeField] public Weapon _gun;

    [SerializeField] Transform firePoint;

    Vector2 firePointPosition;

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
            UseObject(_mousePosition);
            if (!skipWeaponActions)
            {
                DropGun(1.2f);
                TryPickUpGun(_mousePosition);
            }
        }
        skipWeaponActions = false;

        if (Input.GetButton("Fire1"))
        {
            Attack(_mousePosition);
        }

        if (_gun != null)
        {
            _gun.CmdSetLookDirection(lookDirection);
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if (GetComponent<Player>().alive)
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }

        lookDirection = _mousePosition - _rigidbody.position;
        _rigidbody.velocity = new Vector2(_movement.x, _movement.y).normalized * _moveSpeed;
        _rigidbody.rotation = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
    }

    void TryPickUpGun(Vector2 mousePos)
    {
        Debug.Log("CmdTryPickUpGun");
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, gunMask);
        if (hit.collider != null && Vector2.Distance(transform.position, hit.collider.gameObject.transform.position) <= .8f)
        {
            if (hit.collider.TryGetComponent<Weapon>(out Weapon gun))
            {
                Debug.Log("Zaszel wpopu");

                _gun = gun;

                if (!gun.pickUpAvailable)
                {
                    gun = null;
                    return;
                }
                gun.CmdPickUp(transform, firePoint.localPosition);
                gun.RpcPickUp(transform, firePoint.localPosition);
                gun.PickUp(transform, firePoint.localPosition);
            }
        }
    }

    public void DropGun(float gunFlyTime)
    {
        Debug.Log("CmdDropGun");
        if (_gun == null) return;
        _gun.CmdDrop(gunFlyTime);
        _gun.RpcDrop(gunFlyTime);
        _gun.Drop(gunFlyTime);
        _gun = null;
    }

    void Attack(Vector2 mousePos)
    {
        if (_gun == null) return;
        _gun.CmdAttack(mousePos - _rigidbody.position);
    }

    // Œƒ √Œ¬Õ¿
    /*public bool CmdReanimateTeammate(Vector2 mousePos)
    {
        if (!GetComponent<Player>().alive)
        {
            Debug.Log("False");
            return false;
        }

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, playerMask);
        if (hit.collider != null && Vector2.Distance(transform.position, hit.collider.gameObject.transform.position) <= .8f)
        {
            if (hit.collider.TryGetComponent<Player>(out Player player))
            {
                if (player.alive) return false;
                player.alive = true;
                Debug.Log("True");
                return true;
            }
        }

        Debug.Log("False");
        return false;
    }*/

    void UseObject(Vector2 mousePos)
    {
        Debug.Log("UseObject");
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, usableObjectMask);
        if (hit.collider != null && Vector2.Distance(transform.position, hit.collider.gameObject.transform.position) <= 2f)
        {
            if (hit.collider.TryGetComponent<IUsableObject>(out IUsableObject usableObject))
            {
                usableObject.Use();
                skipWeaponActions = true;
            }
        }
    }
}