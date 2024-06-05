using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UI_Controller : NetworkBehaviour
{
    bool _menuIsVisible;

    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ChangeMenuVisibility(_menuIsVisible);
                GetComponent<Player>().canMove = _menuIsVisible;
            }
        }
    }

    void ChangeMenuVisibility(bool menuIsVisible)
    {
        menuIsVisible = !menuIsVisible;
    }
}