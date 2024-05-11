using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class AttackState : BaseState
{
    [SyncVar]
    float moveTimer;
    [SyncVar]
    float losePlayerTimer;


    public override void Enter()
    {

    }

    public override void Exit()
    {
        
    }

    public override void Perform()
    {
        if (!isServer) return;
        if (enemy.CanSeePlayer())
        {
            Debug.Log("PISKI");
            Shoot();

            losePlayerTimer = 0;

            enemy.Agent.destination = enemy.fow.visibleTargets[0].position;

            enemy.LastPlayerKnownPos = enemy.fow.visibleTargets[0].position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if(losePlayerTimer > 4)
            {
                stateMachine.ChangeState(GetComponent<SearchState>());
            }
        }

    }

    public void Shoot()
    {
        enemy.weapon.GetComponent<Weapon>().CmdAttack(enemy.fow.visibleTargets[0].position - enemy.transform.position);
    }
}