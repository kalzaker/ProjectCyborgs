using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngineInternal;

public class Movement : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] LayerMask gunMask;

    Rigidbody2D _rigidbody;
    Camera _camera;

    Vector2 _movement;
    Vector2 _mousePosition;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }
    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Fire2"))
        {
            CheckForGunUnderMouse();
        }

        
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _movement.normalized * _moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDirection = _mousePosition - _rigidbody.position;
        _rigidbody.rotation = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;


        
    }

    void CheckForGunUnderMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, gunMask);
        
        if (hit.collider != null && Vector2.Distance(transform.position, hit.collider.gameObject.transform.position) <= .8f)
        {
            var playerGun = hit.collider.GetComponent<Pistol>();            
            playerGun.firePoint = transform.GetChild(0);
            playerGun.PickUpGun(gameObject);
        }
    }
}
