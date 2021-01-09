using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    void OnEnable()
    {
        direction = General.DirectionToVector(player.GetDirection());
        transform.eulerAngles = Vector3.forward * Mathf.Rad2Deg * Mathf.Atan2(-direction.y, -direction.x);
        time = maxTime;
        ObjectPoolManager.instance.SpawnFromPool("particle_muzzle_flash", transform.position, Quaternion.identity);
    }
}
