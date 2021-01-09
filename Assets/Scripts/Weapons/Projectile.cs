using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed;
    public float range;
    public int damage;
    public LayerMask hittableMask;

    protected Player player;
    protected Vector3 direction;
    protected float maxTime;
    protected float time;
    protected SpriteRenderer spriteRenderer;
    //todo particleeffect



    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = Player.instance;
        maxTime = range / speed;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        transform.position += direction * speed * Time.deltaTime;
        if (time <= 0 && enabled)
        {
            Die(false);
            return;
        }

        checkForCollisions2D();
    }

    public void Die(bool surfaceHit)
    {
        if (!surfaceHit)
        {
            ObjectPoolManager.instance.SpawnFromPool("particle_muzzle_flash", transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }

    void checkForCollisions2D()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, speed * Time.deltaTime, hittableMask.value);
        if (hit.collider != null)
        {
            IHittable[] hittables = hit.collider.GetComponents<IHittable>();
            foreach (IHittable hittable in hittables)
            {
                hittable.OnHit(hit.point, this);
            }

            Die(true);
        }
    }
}
