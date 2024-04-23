using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    float searchTimer;
    float moveTimer;
    public override void Enter()
    {
        enemy.Agent.destination = enemy.LastPlayerKnownPos;
    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
            stateMachine.ChangeState(new AttackState());

        if(enemy.Agent.reachedDestination)
        {
            searchTimer += Time.deltaTime;

            if (searchTimer > 3)
            {
                stateMachine.ChangeState(new PatrolState());
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
