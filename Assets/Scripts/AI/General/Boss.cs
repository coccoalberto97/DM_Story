using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public string bossTag;

    protected override void Die()
    {
        base.Die();
        GameEvents.instance.BossDeath(bossTag);
    }

    public override void OnHit(Vector3 position, Projectile projectile)
    {
        base.OnHit(position, projectile);
        GameEvents.instance.BossHit(getHealth());
    }
}
