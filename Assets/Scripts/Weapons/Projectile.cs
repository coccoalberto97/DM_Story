using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IShootable
{

    public float speed;
    public float range;
    public LayerMask hittableMask;

    private Player player;
    private Vector3 direction;
    private float time;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem hitFlash;



    GameObject IShootable.gameObject()
    {
        return gameObject;
    }

    void IShootable.Shoot()
    {
        hitFlash = GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = Player.instance;
        direction = General.DirectionToVector(player.GetDirection());

        transform.eulerAngles = Vector3.forward * Mathf.Rad2Deg * Mathf.Atan2(-direction.y, -direction.x);
        time = range / speed;
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
            Die();
        }

        checkForCollisions2D();
    }

    public void Die()
    {
        hitFlash.Play();
        spriteRenderer.enabled = false;
        enabled = false;
    }

    void checkForCollisions2D()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, speed * Time.deltaTime, hittableMask.value);
        if (hit.collider != null)
        {
            IHittable hittable = hit.collider.GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.OnHit(hit.point);
            }

            Die();
        }
    }
}
