using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MousePositionCamera : NetworkBehaviour
{
    Camera _camera;
    Transform _player;
    [SerializeField] float _threshold = 0;

    void Start()
    {
        if (!isLocalPlayer) return;
        _camera = Camera.main;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(!isLocalPlayer) return;
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (_player.position + mousePos) / 2f;

        targetPos.x = Mathf.Clamp(targetPos.x, -_threshold + _player.position.x, _threshold + _player.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -_threshold + _player.position.y, _threshold + _player.position.y);

        //this.transform.position = targetPos;

        _ = Input.GetKey(KeyCode.LeftShift) ? _threshold = 3 : _threshold = 0;
    }
}