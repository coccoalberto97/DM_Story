using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageSource : DynamicDamageSource
{

    public float speed;
    public float range;
    protected float maxTime;
    protected float time;
    //todo particleeffect

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Awake()
    {
        maxTime = range / speed;
    }

    public virtual void Die(bool surfaceHit)
    {
        gameObject.SetActive(false);
    }

    public override bool CheckForCollisions2D()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, speed * Time.deltaTime, hittableMask.value);
        if (hit.collider != null)
        {
            IHittable[] hittables = hit.collider.GetComponents<IHittable>();
            foreach (IHittable hittable in hittables)
            {
                hittable.OnHit(hit.point, this);
            }
            return true;
            //Die(true);
        }
        return false;
    }

    public virtual void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0 && enabled)
        {
            Die(false);
            return;
        }
    }
}
