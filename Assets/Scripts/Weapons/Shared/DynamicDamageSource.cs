using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class DynamicDamageSource : MonoBehaviour
{
    public int damage;
    public LayerMask hittableMask;
    public DamageTypeEnum weaponDamageType = DamageTypeEnum.DEFAULT;

    protected Player player;
    protected Vector3 direction;
    protected SpriteRenderer spriteRenderer;

    protected virtual void OnEnable()
    {
        player = Player.instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public abstract bool CheckForCollisions2D();
}
