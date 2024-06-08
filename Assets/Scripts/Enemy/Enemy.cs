using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
using Mirror;
using System;

public class Enemy : NetworkBehaviour, IHitable
{
    StateMachine stateMachine;
    IAstarAI agent;
    Vector3 lastPlayerKnownPos;
    [HideInInspector] public FieldOfView fow;
    public IAstarAI Agent { get => agent; }
    public Vector3 LastPlayerKnownPos { get => lastPlayerKnownPos; set => lastPlayerKnownPos = value; }

    public Path path;
    [SerializeField] string currentState;
    public GameObject debugPlayerPosPoint;

    [SyncVar]
    Vector2 lookDirection;

    public bool canSeePlayer;


    [Header("Weapon")]
    [SerializeField]
    GameObject weaponToSpawn;

    [SyncVar]
    public Weapon weapon;

    [SerializeField] Transform firePoint;

    public event Action<Enemy> OnEnemyKilled;

    void Start()
    {
        if (isServer)
        {
            EnemyManager.Instance.RegisterEnemy(this);
        }

        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<IAstarAI>();
        stateMachine.Initialise();
        fow = GetComponent<FieldOfView>();

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        CmdSpawnGun();
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnGun()
    {
        GameObject tempWeapon = Instantiate(weaponToSpawn, transform.position, Quaternion.identity);
        NetworkServer.Spawn(tempWeapon);

        RpcAssignWeapon(tempWeapon);
    }

    [ClientRpc]
    void RpcAssignWeapon(GameObject weaponObject)
    {
        weapon = weaponObject.GetComponent<Weapon>();
        weapon.isInEnemiesHands = true;

        weapon.PickUp(transform, firePoint.localPosition);
        weapon.pickUpAvailable = false;
        EnemyManager.Instance.enemiesWeapons.Add(weapon);
    }

    void Update()
    {
        if (!isServer) return;
        currentState = stateMachine.activeState.ToString();
        debugPlayerPosPoint.transform.position = lastPlayerKnownPos;


        if (weapon == null) return;
        weapon.SetLookDirection(firePoint.transform.position - gameObject.transform.position);
    }

    public bool CanSeePlayer()
    {
        return fow.visibleTargets.Count > 0;
    }

    
    public void Hit()
    {
        CmdHit();
    }

    [Command(requiresAuthority = false)]
    void CmdHit()
    {
        Die();
    }

    void Die()
    {
        if (weapon != null)
        {
            weapon.Drop(0);
            weapon = null;
        }

        OnEnemyKilled?.Invoke(this);
        NetworkServer.Destroy(gameObject);
    }
}