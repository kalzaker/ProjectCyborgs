using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrolState : BaseState
{
    float waitTimer;
    public int WaypointIndex;
    public override void Enter()
    {
        enemy.Agent.speed = 1;
    }

    public override void Exit()
    {
        
    }

    public override void Perform()
    {
        PatrolCycle();
        if(enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public void PatrolCycle()
    {
        if(enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer > 3)
            {
                if (WaypointIndex < enemy.path.waypoints.Count - 1)
                    WaypointIndex++;
                else
                    WaypointIndex = 0;
                enemy.Agent.SetDestination(enemy.path.waypoints[WaypointIndex].position);
                waitTimer = 0;
            }
        }
        if (enemy.Agent.velocity != Vector3.zero)
        {
            Vector3 moveDirection = new Vector3(enemy.Agent.velocity.x, enemy.Agent.velocity.y, 0f);
            if (moveDirection != Vector3.zero)
            {
                float angel = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angel, Vector3.back);
                enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, targetRotation, 200 * Time.deltaTime);
            }
        }

    }
}
