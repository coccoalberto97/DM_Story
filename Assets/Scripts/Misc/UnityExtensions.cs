using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensions
{

    /// <summary>
    /// Extension method to check if a layer is in a layermask
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="damage"></param>
    /// <param name="mFactor">Modificatore del danno basato sugli elementi, per ottenerlo <see cref="General.GetElementEffectiveness(DamageTypeEnum, DamageTypeEnum)"/></param>
    /// <returns></returns>
    public static int CalculateDamage(this Entity entity, int damage, float mFactor)
    {
        int dmgAmount = mFactor == 0 || (damage * mFactor) % 1 == 0 ? (int)(damage * mFactor) : (int)(damage * mFactor) + 1;
        return dmgAmount;
    }
}
