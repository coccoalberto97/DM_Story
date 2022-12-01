using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Controller2D))]
public class Player : Entity, IHittable
{

    public float maxJumpHeight = 3f;
    public float minJumpHeight = 1.5f;
    public float timeToJumpApex = .4f;
    public float accelerationTimeGrounded = .1f;
    public float accelerationTimeAirborneMultiplier = 2f;

    public float timeInvincible = 2f;
    public bool invincible;
    public bool forceApplied;

    public float moveSpeed = 6f;
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    Vector2 movementInput;
    float velocityXSmoothing;

    Direction direction;
    bool facingRight;

    Animator animator;
    SpriteRenderer spriteRenderer;
    public GameObject interactableIcon;

    KeyCode jumpkey = KeyCode.Z;

    Controller2D controller;
    public static Player instance;

    public Vector2 colliderSize = new Vector2(1.5f, 3);

    private int currentExp = 0;
    public int maxExp = 100;
    private bool inputEnabled = true;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }

    public Direction GetDirection()
    {
        return direction;
    }

    public void ApplyDamage(int damage)
    {
        if (!invincible)
        {
            SubtractHealth(damage);
            //devo castare a double per non perdere la precisione
            double expFactor = (double)damage * 100 / maxHealth;
            SubtractExp((int)Math.Floor(expFactor));
            Debug.Log($"Damage {damage} exp lost {expFactor}");
            SetVelocity(Vector2.up * 16.0f);
            StartCoroutine(SetInvincible());
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<Controller2D>();
        //Initialize Vertical Values
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    private void Update()
    {
        if (!inputEnabled)
        {
            return;
        }
        GetInput();
        Animation();
        Horizontal();
        Vertical();
        ApplyMovement();
        GetKey();
    }

    private void GetInput()
    {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        direction = facingRight ? Direction.RIGHT : Direction.LEFT;

        if (movementInput.x != 0)
        {
            direction = (movementInput.x > 0 ? Direction.RIGHT : Direction.LEFT);
            facingRight = movementInput.x > 0;
        }

        float verticalAimFactor = movementInput.y;
        if (controller.collisions.below)
        {
            verticalAimFactor = Mathf.Clamp01(verticalAimFactor);
        }


        if (verticalAimFactor != 0)
        {
            direction = (verticalAimFactor > 0 ? Direction.UP : Direction.DOWN);
            //facingRight = movementInput.x > 0;
        }

    }

    private void Animation()
    {
        spriteRenderer.flipX = facingRight;
        animator.SetFloat("VelocityX", Mathf.Abs(movementInput.x));
        animator.SetFloat("VelocityY", Mathf.Sign(velocity.y));
        animator.SetFloat("Looking", General.DirectionToVector(direction).y);
        animator.SetBool("Grounded", controller.collisions.below);
    }

    private void Vertical()
    {
        if (forceApplied)
        {
            forceApplied = false;
        }
        else if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (Input.GetKeyDown(jumpkey) && controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
        }

        if (Input.GetKeyUp(jumpkey))
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }

        velocity.y += gravity * Time.deltaTime;

    }

    private void Horizontal()
    {
        float targetVelocityX = movementInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            accelerationTimeGrounded * (controller.collisions.below ? 1.0f : accelerationTimeAirborneMultiplier));
    }

    private void ApplyMovement()
    {
        controller.Move(velocity * Time.deltaTime);
    }


    private void GetKey()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CheckInteraction();
        }
    }

    private void SetVelocity(Vector2 v)
    {
        velocity = v;
        forceApplied = true;
    }

    private IEnumerator SetInvincible()
    {
        invincible = true;
        float elapsedTime = 0f;
        while (elapsedTime < timeInvincible)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            elapsedTime += 0.04f;
            yield return new WaitForSeconds(0.04f);
        }

        spriteRenderer.enabled = true;
        invincible = false;
    }

    protected override void ModHealth(int m)
    {
        base.ModHealth(m);
        GameEvents.instance.PlayerModHealth();
    }

    public void OnHit(Vector3 position, DynamicDamageSource projectile)
    {
        ApplyDamage(projectile.damage);
    }

    protected void ModExp(int exp)
    {
        currentExp += exp;
        if (currentExp <= 0)
        {
            currentExp = 0;
        }
        else if (currentExp > maxExp)
        {
            currentExp = maxExp;
        }
        GameEvents.instance.PlayerModExp();
    }

    public void LoadData(SaveData data)
    {
        if (data != null)
        {
            this.maxHealth = data.maxHp;
            this.setHealth(data.currentHp);
            this.transform.position = new Vector3(data.currentX, data.currentY, 0);
        }
    }

    public void ToggleInteractableIcon(bool active)
    {
        this.interactableIcon.SetActive(active);
    }

    private void CheckInteraction()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, colliderSize, 0, Vector2.zero);
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if (interactable)
                {
                    interactable.Interact();
                }
            }
        }
    }

    public void AddExp(int exp)
    {
        ModExp(exp);
    }

    public void SubtractExp(int exp)
    {
        ModExp(-exp);
    }

    public int GetExp()
    {
        return currentExp;
    }

    public void SetInputEnabled(bool inputEnabled) {
        this.inputEnabled = inputEnabled;
    }

    public bool GetInputEnabled() {
        return this.inputEnabled;
    }
}
