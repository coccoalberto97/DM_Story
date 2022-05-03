using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemyProjectile : ProjectileDamageSource
{

    public Transform target;
    public bool direct;
    public string timeoutMuzzleFlash = "particle_muzzle_flash";

    protected override void OnEnable()
    {
        base.OnEnable();
        if (!target)
        {
            target = player.transform;
        }

        if (direct)
        {
            direction = new Vector3(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y, 1).normalized;
        }
        else
        {

            float xDif = target.transform.position.x - transform.position.x;
            float yDif = target.transform.position.y - transform.position.y;
            if (xDif > 0.8f)
            {
                direction = General.DirectionToVector(Direction.RIGHT);
            }
            else if (xDif < -0.8f)
            {
                direction = General.DirectionToVector(Direction.LEFT);
            }
            else if (yDif > 0.8f)
            {
                direction = General.DirectionToVector(Direction.UP);

            }
            else if (yDif < 0.8f)
            {
                direction = General.DirectionToVector(Direction.DOWN);
            }
        }

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

    public override void Die(bool surfaceHit)
    {
        if (!surfaceHit && !string.IsNullOrEmpty(timeoutMuzzleFlash))
        {
            ObjectPoolManager.instance.SpawnFromPool(timeoutMuzzleFlash, transform.position, Quaternion.identity);
        }
        base.Die(surfaceHit);
    }

    public override void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        base.Update();
        CheckForCollisions2D();
    }
}
