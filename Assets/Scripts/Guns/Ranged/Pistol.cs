using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Pistol : RangedWeapon
{
    [Command]
    public override void CmdAttack(Vector2 shootDirection)
    {
        base.CmdAttack(shootDirection);
    }
}