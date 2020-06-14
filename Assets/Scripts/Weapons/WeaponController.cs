using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon weapon;
    private Player player;
    private SpriteRenderer spriteRenderer;
    private Transform weaponTransform;

    void Start()
    {
        player = Player.instance;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        weaponTransform = spriteRenderer.transform;
        initGun();
    }


    void Update()
    {
        handleDirection();
        Shooting();
    }

    private void initGun()
    {
        spriteRenderer.sprite = weapon.sprite;
    }

    private void handleDirection()
    {
        bool facingRight = player.IsFacingRight();
        Direction direction = player.GetDirection();

        spriteRenderer.flipX = facingRight;
        Vector2 handleOffset = new Vector2(weapon.handleOffsetX, weapon.handleOffestY);

        float facingRightMultiplier = facingRight ? -1 : 1;
        float facingUpMultiplier = direction == Direction.UP ? 1 : -1;

        float x = 0;
        float y = 0;
        float rot = 0;

        if (direction == Direction.UP || direction == Direction.DOWN)
        {
            x = handleOffset.x * 2;
            y = facingRightMultiplier * (handleOffset.y);
            rot = facingUpMultiplier * -90f;
        }
        else
        {
            x = handleOffset.x;
            y = facingRightMultiplier * handleOffset.y;
            rot = 90f - (facingRightMultiplier * 90f);
        }

        spriteRenderer.flipX = false;
        spriteRenderer.flipY = facingRight;

        weaponTransform.localPosition = new Vector3(x, y, 0) * 0.0625f;
        transform.eulerAngles = new Vector3(0, 0, rot);
    }

    private void Shooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject shootableObject = Instantiate(weapon.shootablePrefab, weaponTransform.position, Quaternion.identity);
            IShootable shootable = shootableObject.GetComponent<IShootable>();
            shootable.Shoot();
        }
    }
}
