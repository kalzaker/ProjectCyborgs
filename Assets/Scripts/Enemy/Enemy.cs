using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class Enemy : MonoBehaviour
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

    [Header("Weapon")]
    public Transform Gun;
    [Range(.1f, 10f)]
    public float fireRate;

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<IAstarAI>();
        stateMachine.Initialise();
        fow = GetComponent<FieldOfView>();
    }

    void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
        debugPlayerPosPoint.transform.position = lastPlayerKnownPos;
    }
    public bool CanSeePlayer()
    {
        if(fow.visibleTargets.Count > 0)
        {
            return true;
        }
        return false;
    }
}
