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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeMenuVisibility();
            Debug.Log("Bebra");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (!GetComponent<Player>().canMove) return;

        _mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Fire2"))
        {
            DropGun(1.2f);
            TryPickUpGun(_mousePosition);
        }

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
        if (!GetComponent<Player>().canMove) {
            _rigidbody.velocity = Vector2.zero;
            return;
        }
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

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

                EventManager.weaponPickedUp.Invoke(gun);

                if (!isServer)
                {
                    gun.CmdPickUp(transform, firePoint.localPosition);
                    gun.PickUp(transform, firePoint.localPosition);
                    gun.CmdPlayPickUpSound();
                    return;
                }
                else
                {
                    gun.RpcPickUp(transform, firePoint.localPosition);
                    gun.PickUp(transform, firePoint.localPosition);
                    gun.RpcPlayPickUpSound();
                    return;
                }
            }
        }
    }

    public void DropGun(float gunFlyTime)
    {
        Debug.Log("CmdDropGun");
        if (_gun == null) return;

        EventManager.weaponDropped.Invoke();

        if (!isServer)
        {
            _gun.CmdDrop(gunFlyTime);
            _gun.Drop(gunFlyTime);
        }
        if (isServer)
        {
            _gun.RpcDrop(gunFlyTime);
            _gun.Drop(gunFlyTime);
        }
        _gun = null;
    }

    void Attack(Vector2 mousePos)
    {
        if (_gun == null) return;
        _gun.Attack(mousePos - _rigidbody.position);
    }


    void ChangeMenuVisibility()
    {
        UI_Controller.instance.ChangeMenuVisibility();
        if (!GetComponent<Player>().alive)
        {
            return;
        }
        GetComponent<Player>().canMove = !UI_Controller.instance.menuIsVisible;
    }

    void RestartGame()
    {
        UI_Controller.instance.RestartGame();
    }
}