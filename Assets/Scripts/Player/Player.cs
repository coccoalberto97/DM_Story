using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Controller2D))]
public class Player : Entity
{

    public float maxJumpHeight = 3f;
    public float minJumpHeight = 1.5f;
    public float timeToJumpApex = .4f;
    public float accelerationTimeGrounded = .1f;
    public float accelerationTimeAirborneMultiplier = 2f;

    public float timeInvincible = 2f;
    public bool invincible;
    public bool forceApplied;

    float moveSpeed = 6f;
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

    KeyCode jumpkey = KeyCode.Z;

    Controller2D controller;
    public static Player instance;

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
            Debug.Log("Damage " + damage);
            SetVelocity(Vector2.up * 16.0f);
            StartCoroutine(setInvincible());
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
        GetInput();
        Animation();
        Horizontal();
        Vertical();
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



    private void SetVelocity(Vector2 v)
    {
        velocity = v;
        forceApplied = true;
    }

    private IEnumerator setInvincible()
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
}
