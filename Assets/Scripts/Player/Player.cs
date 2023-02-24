using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController2D))]
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
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    Vector2 movementInput;

    Direction direction;
    bool facingRight;

    Animator animator;
    SpriteRenderer bodyRenderer;
    public SpriteRenderer armsRenderer;

    public GameObject interactableIcon;

    KeyCode jumpkey = KeyCode.Z;

    CharacterController2D controller;
    Rigidbody2D rigidBody;
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
            // TODO fix dmg
            //
            // SetVelocity(Vector2.up * 16.0f);
            StartCoroutine(SetInvincible());
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        bodyRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<CharacterController2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        //Initialize Vertical Values
        float jumpFactor = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(jumpFactor) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(jumpFactor) * minJumpHeight);
    }

    private void Update()
    {
        if (!inputEnabled)
        {
            return;
        }
        Horizontal();
        Vertical();
        GetInput();
        GetKey();
    }

    private void FixedUpdate()
    {
        if (!inputEnabled)
        {
            return;
        }
        Animation();
        ApplyMovement();
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
        if (controller.IsGrounded())
        {
            verticalAimFactor = Mathf.Clamp01(verticalAimFactor);
        }


        if (verticalAimFactor != 0)
        {
            direction = (verticalAimFactor > 0 ? Direction.UP : Direction.DOWN);
        }

    }

    private void Animation()
    {
        bodyRenderer.flipX = facingRight;
        armsRenderer.flipX = facingRight;
        animator.SetFloat("VelocityX", Mathf.Abs(rigidBody.velocity.x));
        animator.SetFloat("VelocityY", Mathf.Sign(rigidBody.velocity.y));
        animator.SetFloat("Looking", General.DirectionToVector(direction).y);
        animator.SetBool("Grounded", controller.IsGrounded());
    }

    private void Horizontal() {
        velocity.x = movementInput.x * moveSpeed;
    }

    private void Vertical()
    {
        if (forceApplied)
        {
            forceApplied = false;
        }
       
        if (Input.GetKeyDown(jumpkey) && controller.IsGrounded())
        {
            velocity.y = maxJumpVelocity;
        }
    }

    private void ApplyMovement()
    {
        controller.Move(velocity.x * Time.fixedDeltaTime, velocity.y > 0);
        velocity.y = 0;
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
            bodyRenderer.enabled = !bodyRenderer.enabled;
            armsRenderer.enabled = !armsRenderer.enabled;
            elapsedTime += 0.04f;
            yield return new WaitForSeconds(0.04f);
        }

        bodyRenderer.enabled = true;
        armsRenderer.enabled = true;
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
