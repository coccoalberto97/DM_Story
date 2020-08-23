using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpEnemy1Behaviour : Enemy
{

    SpriteRenderer spriteRenderer;
    float topY;
    float bottomY;
    float x;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        x = transform.position.x;
        topY = transform.position.y + 1.5f;
        bottomY = transform.position.y - 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        LookAtPlayer();
    }

    void Movement()
    {
        float t = Mathf.Sin(1.5f * Time.time) / 2.0f + 0.5f;

        transform.position = new Vector3(x, Mathf.Lerp(bottomY, topY, t), 0f);
    }

    void LookAtPlayer()
    {
        spriteRenderer.flipX = player.transform.position.x > x;
    }
}
