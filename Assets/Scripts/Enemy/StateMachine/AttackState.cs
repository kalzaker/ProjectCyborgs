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
        enemy.Agent.speed = 2;
    }

    public override void Exit()
    {
        
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;

            Vector2 direction = (enemy.fow.visibleTargets[0].position - enemy.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            enemy.transform.rotation = Quaternion.Euler(Vector3.forward * (angle - 90f));

            if (shotTimer > enemy.fireRate)
                Shoot();

            if(moveTimer > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 3));
                moveTimer = 0;
            }

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
