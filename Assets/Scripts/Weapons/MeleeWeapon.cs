using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MeleeWeapon : Weapon
{
    [SerializeField] protected float attackRange;
    [SerializeField] protected LayerMask enemyLayer;

    [Command(requiresAuthority = false)]
    public override void CmdAttack(Vector2 shootDirection)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(attackPoint.position.x, attackPoint.position.y), attackRange, enemyLayer);
        if (hitColliders.Length == 0) return;
        
        foreach (Collider2D collider in hitColliders)
        {
            if(TryGetComponent<IHitable>(out IHitable target))
            {
                target.Hit();
                Debug.Log("GAY");
            }
        }
    }
}
