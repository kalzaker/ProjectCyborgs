using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackState : BaseState
{
    float moveTimer;
    float losePlayerTimer;
    float shotTimer;

    public override void Enter()
    {

    }

    public override void Exit()
    {
        
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            shotTimer += Time.deltaTime;

            enemy.Agent.destination = enemy.fow.visibleTargets[0].position;

            if (shotTimer > enemy.fireRate)
                Shoot();

            enemy.LastPlayerKnownPos = enemy.fow.visibleTargets[0].position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if(losePlayerTimer > 4)
            {
                stateMachine.ChangeState(new SearchState());
            }
        }

    }

    public void Shoot()
    {
        Transform gun = enemy.Gun;
        GameObject bullet = GameObject.Instantiate(Resources.Load("Objects/Enemy/Gun/EnemyBullet") as GameObject, gun.position, enemy.transform.rotation);
        Vector2 shootDirection = (enemy.fow.visibleTargets[0].position - enemy.transform.position).normalized;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(Random.Range(-3f,3f),Vector2.up) * shootDirection * 40;
        Debug.Log("¡¿Ã ¡Àﬂ");
        shotTimer = 0;
    }
}
