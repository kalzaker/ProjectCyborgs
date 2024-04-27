using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class MousePositionCamera : NetworkBehaviour
{
    Camera _camera;
    CinemachineVirtualCamera _virtualCamera;
    [SerializeField] Transform _player;
    float _threshold = 0;

    public override void OnStartLocalPlayer()
    {
        if (!isLocalPlayer) return;
        _camera = Camera.main;
        _virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        _virtualCamera.Follow = this.gameObject.transform;
    }

    void Update()
    {
        if(!isLocalPlayer) return;
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = new Vector3(_player.position.x + mousePos.x, _player.position.y + mousePos.y, _player.position.z - 3) / 2f;

        targetPos.x = Mathf.Clamp(targetPos.x, -_threshold + _player.position.x, _threshold + _player.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -_threshold + _player.position.y, _threshold + _player.position.y);

        _camera.transform.position = targetPos;

        _ = Input.GetKey(KeyCode.LeftShift) ? _threshold = 3 : _threshold = 0;
    }
}