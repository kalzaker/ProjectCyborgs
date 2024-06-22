using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UI_Controller : NetworkBehaviour
{
    public static UI_Controller instance;

    [SerializeField] GameObject deathScreen;

    [SerializeField] GameObject menu;

    [SerializeField] GameObject restartButton;


    public bool menuIsVisible;

    [SyncVar]
    bool gameOver;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isServer)
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        EventManager.playerDied.AddListener(OnPlayerDie);
    }

    public void ChangeMenuVisibility()
    {
        if (menuIsVisible)
        {
            SetMenuNotActive();
            return;
        }
        else
        {
            SetMenuActive();
            return;
        }
    }

    void SetMenuActive()
    {
        menuIsVisible = true;
        menu.SetActive(true);
        if (isServer)
        {
            restartButton.SetActive(true);
        }
    }

    void SetMenuNotActive()
    {
        menuIsVisible = false;
        menu.SetActive(false);
        if (isServer)
        {
            restartButton.SetActive(false);
        }
    }

    void OnPlayerDie()
    {
        CmdShowDeathScreen();
    }

    [Command(requiresAuthority = false)]
    void CmdShowDeathScreen()
    {
        RpcShowDeathScreen();
    }

    [ClientRpc]
    void RpcShowDeathScreen()
    {
        ShowDeathScreen();
    }

    void ShowDeathScreen()
    {
        foreach(Player player in FindObjectsOfType<Player>())
        {
            player.canMove = false;
        }
        deathScreen.SetActive(true);
        gameOver = true;
    }

    public void RestartGame()
    {
        RpcRestartGame();
    }

    [ClientRpc]
    void RpcRestartGame()
    {
        if (!gameOver) return;

        MapManager.Instance.RestartGame();
        gameOver = false;
        deathScreen.SetActive(false);
        foreach (Player player in FindObjectsOfType<Player>())
        {
            player.GetComponent<Player>().canMove = true;
            player.GetComponent<Player>().alive = true;
        }
    }
}