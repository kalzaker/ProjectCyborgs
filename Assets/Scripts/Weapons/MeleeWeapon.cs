using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MeleeWeapon : Weapon
{
    [SerializeField] protected float attackRange;

    [SerializeField] AudioClip hitSound;

    public override void Attack(Vector2 shootDirection)
    {
        CmdAttack(shootDirection);
    }

    [Command(requiresAuthority = false)]
    void CmdAttack(Vector2 shootDirection)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(attackPoint.position.x, attackPoint.position.y), attackRange, enemyLayer);
        if (hitColliders.Length == 0) return;
        
        foreach (Collider2D collider in hitColliders)
        {
            if(collider.TryGetComponent<IHitable>(out IHitable target))
            {
                if (!collider.TryGetComponent<Player>(out _))
                {
                    audioPlayer.PlaySound(hitSound);

                    target.Hit();
                    Debug.Log("GAY");
                }
            }
        }
    }
}
