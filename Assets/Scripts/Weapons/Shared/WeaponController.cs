using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public List<Weapon> weaponsOwned;
    private int equippedWeaponId;
    private Weapon equippedWeapon;
    private Player player;
    private SpriteRenderer spriteRenderer;
    private Transform weaponTransform;

    public void Start()
    {
        player = Player.instance;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        weaponTransform = spriteRenderer.transform;
        GameEvents.instance.OnPlayerModExp += OnPlayerModExp;
        InitWeapon();
    }

    public void Update()
    {
        if (!player.GetInputEnabled()) {
            return;
        }
        CheckSwitchgWeapon();
        HandleDirection();
        Shooting();
    }

    private void InitWeapon()
    {
        equippedWeapon = weaponsOwned[equippedWeaponId];
        spriteRenderer.sprite = equippedWeapon.sprite;
        equippedWeapon.SetShootable(Player.instance.GetExp());
    }

    private void CheckSwitchgWeapon()
    {
        int weaponIdMod = 0;

        if (Input.GetButtonDown("Fire3"))
        {
            weaponIdMod = -1;
        }
        else if (Input.GetButtonDown("Fire4"))
        {
            weaponIdMod = 1;
        }

        if (weaponIdMod != 0)
        {
            equippedWeaponId = (equippedWeaponId + weaponIdMod + weaponsOwned.Count) % weaponsOwned.Count;
            InitWeapon();
        }
    }

    private void HandleDirection()
    {
        bool facingRight = player.IsFacingRight();
        Direction direction = player.GetDirection();

        spriteRenderer.flipX = facingRight;
        Vector2 handleOffset = new Vector2(equippedWeapon.handleOffsetX, equippedWeapon.handleOffestY);

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
        if (Input.GetButtonDown("Fire1")) // Input.GetButton("Fire1") for automatic
        {
            if (equippedWeapon.currentDescriptor != null)
            {
                ObjectPoolManager.instance.SpawnFromPool(equippedWeapon.currentDescriptor.shootablePrefabTag, weaponTransform.position, weaponTransform.rotation, false);
            }
        }
    }

    private void OnPlayerModExp()
    {
        equippedWeapon.SetShootable(Player.instance.GetExp());
    }

}
