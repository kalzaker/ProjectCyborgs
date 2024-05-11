using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SearchState : BaseState
{
    [SyncVar]
    float searchTimer;
    [SyncVar]
    float moveTimer;
    public override void Enter()
    {
        if (!isServer) return;
        enemy.Agent.destination = enemy.LastPlayerKnownPos;
    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        if (!isServer) return;
        if (enemy.CanSeePlayer())
            stateMachine.ChangeState(GetComponent<AttackState>());

        if(enemy.Agent.reachedDestination)
        {
            searchTimer += Time.deltaTime;

            if (searchTimer > 3)
            {
                stateMachine.ChangeState(GetComponent<PatrolState>());
            }
        }

        //if (enemy.Agent.velocity != Vector3.zero)
        //{
        //    Vector3 moveDirection = new Vector3(enemy.Agent.velocity.x, enemy.Agent.velocity.y, 0f);
        //    if (moveDirection != Vector3.zero)
        //    {
        //        float angel = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
        //        Quaternion targetRotation = Quaternion.AngleAxis(angel, Vector3.back);
        //        enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, targetRotation, 200 * Time.deltaTime);
        //    }
        //}
    }
}
