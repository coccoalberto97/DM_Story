using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardPlayerProjectile : ProjectileDamageSource
{
    public string timeoutMuzzleFlash = "particle_muzzle_flash";

    protected override void OnEnable()
    {
        base.OnEnable();
        direction = General.DirectionToVector(player.GetDirection());
        transform.eulerAngles = Vector3.forward * Mathf.Rad2Deg * Mathf.Atan2(-direction.y, -direction.x);
        time = maxTime;
        ObjectPoolManager.instance.SpawnFromPool("particle_muzzle_flash", transform.position, Quaternion.identity);
    }

    public override bool CheckForCollisions2D()
    {
        if (base.CheckForCollisions2D())
        {
            Die(true);
            return true;
        }
        return false;
    }

    public override void Update()
    {

        transform.position += direction * speed * Time.deltaTime;
        base.Update();
        CheckForCollisions2D();
    }

    public override void Die(bool surfaceHit)
    {
        if (!surfaceHit && !string.IsNullOrEmpty(timeoutMuzzleFlash))
        {
            ObjectPoolManager.instance.SpawnFromPool(timeoutMuzzleFlash, transform.position, Quaternion.identity);
        }
        base.Die(surfaceHit);
    }
}
