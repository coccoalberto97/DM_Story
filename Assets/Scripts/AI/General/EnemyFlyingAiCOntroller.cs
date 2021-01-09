using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingAiController : Enemy
{

    [Header("PathFinder")]
    public Transform movementTarget;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("RangedSettings")]
    public Transform shootingTarget;
    public float shootingRange;
    public float distToTarget; //la distanza alla quale inizia ad avvicinarsi per entrare in range
    public float bulletCooldown;
    public string shootablePrefabTag;
    private bool canShoot = true;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    /*TODO Dash
     * public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;*/

    [Header("Behavior")]
    public bool followEnabled = true;
    public bool shootingEnabled = false;
    public bool dashEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    //private bool isGrounded = false;
    private bool reachedPathEnd;
    private Seeker seeker;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
        if (!shootingTarget)
        {
            shootingTarget = Player.instance.transform;
        }

        if (!movementTarget)
        {
            movementTarget = Player.instance.transform;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TargetIsInRange() && followEnabled)
        {
            Move();
        }

        if (shootingEnabled && TargetIsInShootingRange())
        {
            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }

    }

    void Move()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedPathEnd = true;
            return;
        }
        else
        {
            reachedPathEnd = false;
        }


        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        while (direction.x == 0f && currentWaypoint < path.vectorPath.Count)
        {
            currentWaypoint++;
            direction = 10f * ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        }

        Vector2 force = direction * speed * Time.deltaTime;

        // TODO Dash
        /*
        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }*/

        // Movement
        rb.AddForce(force);
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetIsInRange()
    {
        return Vector2.Distance(transform.position, movementTarget.transform.position) < activateDistance;
    }


    private IEnumerator Shoot()
    {
        ObjectPoolManager.instance.SpawnFromPool(shootablePrefabTag, transform.position, Quaternion.identity, false);
        canShoot = false;
        yield return new WaitForSeconds(bulletCooldown);
        canShoot = true;
    }

    private bool TargetIsInShootingRange()
    {
        return Vector2.Distance(transform.position, shootingTarget.transform.position) < shootingRange;
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, movementTarget.position, OnPathComplete);
        }
    }
}
