using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
using Mirror;

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
    [SerializeField] GameObject debugPlayerPosPoint;

    [SyncVar]
    Vector2 lookDirection;

    public bool canSeePlayer;



    [Header("Weapon")]
    [SerializeField]
    GameObject weaponToSpawn;

    [SyncVar]
    public Weapon weapon;

    [SerializeField] Transform firePoint;

    void Start()
    {
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
        RpcDie();
    }

    [ClientRpc]
    void RpcDie()
    {
        if (weapon != null)
        {
            weapon.Drop(0);
            weapon = null;
        }

        Destroy(gameObject);
    }
}