using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionCamera : MonoBehaviour
{
    Camera _camera;
    [SerializeField] Transform _player;
    [SerializeField] float _threshold = 0;

    void Start()
    {
        _camera = Camera.main;    
    }
    void Update()
    {
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (_player.position + mousePos) / 2f;

        targetPos.x = Mathf.Clamp(targetPos.x, -_threshold + _player.position.x, _threshold + _player.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -_threshold + _player.position.y, _threshold + _player.position.y);

        this.transform.position = targetPos;

        _ = Input.GetKey(KeyCode.LeftShift) ? _threshold = 3 : _threshold = 0;


    }
}
