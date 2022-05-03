using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IHittable
{
    public float dropChance = 0.15f;
    public List<string> droppables = new List<string>(); //lista di elementi droppabili
    public int expDrop = 5;

    public virtual void OnHit(Vector3 position, DynamicDamageSource projectile)
    {
        //calcolo il danno con il moltiplicatore del danno
        ElementEffectiveness  elementEffectiveness = General.GetElementEffectiveness(projectile.weaponDamageType, entityElement);
        int dmg = this.CalculateDamage(projectile.damage, elementEffectiveness.mFactor);
        DamageIndicator.Spawn(dmg, transform.position, elementEffectiveness);
        SubtractHealth(dmg);
    }

    protected override void Die()
    {
        base.Die();
        //calcolo se devo droppare o meno
        if (droppables != null && droppables.Count > 0)
        {
            float randValue = Random.value;
            if (randValue < dropChance)
            {
                //estraggo dalla pool un numero a caso
                int index = Random.Range(0, droppables.Count - 1);
                ObjectPoolManager.instance.SpawnFromPool(droppables[index], transform.position, Quaternion.identity);
            }
        }

        if (expDrop > 0)
        {
            Player.instance.AddExp(expDrop);
        }

        gameObject.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
