using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    //basic movement
    public float movementSpeed = 6f;
    public float turnSpeed = 4f;
    public AnimationCurve followCurve;
    public float maxFollowDist = 5f;
    public float minFollowDist = 1f;
    private float followDist;
    private bool facingRight = false;
    private Vector2 movementVector = Vector2.zero;


    //dash
    public float dashSpeed = 20;
    public AnimationCurve dashSpeedCurve;

    private bool dashing = false;
    private Vector2 dashDirection = Vector2.zero;
    [SerializeField] private float dashTimer = 0f;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float chainTime = 0.5f;
    [SerializeField] private float timeSinceDashEnd = Mathf.Infinity;



    //dash cooldown
    [SerializeField] public float failedDashCooldownTime = 2;




    //components
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Animator animator;

    PlayerCombat playerState;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerState = GetComponent<PlayerCombat>();
    }

    void StartDash(Vector2 direction)
    {
        dashing = true;
        dashTimer = dashTime;
        timeSinceDashEnd = Mathf.Infinity;
        dashDirection = direction;
        animator.SetTrigger("startDash");

    }

    void StopDash()
    {
        dashing = false;
        dashTimer = 0f;
        timeSinceDashEnd = 0f; // Start chain window
        body.velocity = Vector2.zero;
        animator.SetTrigger("endDash");

    }

    public Vector2 getMoveDirection()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector2 mouseWorldPoint = (Vector2)Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 playerWorldPoint = (Vector2)transform.position;
        Vector2 mouseDist = mouseWorldPoint - playerWorldPoint;
        Vector2 newMovementVector = mouseDist.normalized;
        return newMovementVector;
    }

    void Update()
    {
        if (playerState.IsStunned())
        {
            return;
        }
        if (dashing || timeSinceDashEnd < chainTime)
        {
            dashCounter.text = dashCount.ToString();
            dashCounter.enabled = true;
        }
        else dashCounter.enabled = false;


        Vector2 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = mouseWorldPoint - (Vector2)transform.position;
        Vector2 newMovementVector = toMouse.normalized;


        if (Input.GetKeyDown(KeyCode.Space) && !dashing)
        {
            bool canChain = timeSinceDashEnd < chainTime;
            bool canStartNew = timeSinceDashEnd > failedDashCooldownTime;

            if (canChain || canStartNew) StartDash(newMovementVector);

        }


        followDist = Mathf.Min(toMouse.magnitude, maxFollowDist);
        if (followDist < minFollowDist) followDist = 0;

        movementVector = Vector2.Lerp(movementVector, newMovementVector, Time.deltaTime * turnSpeed);

        if (movementVector.x != 0)
        {
            facingRight = movementVector.x > 0;
            sprite.flipX = !facingRight;
        }
    }

    void FixedUpdate()
    {
        if (playerState.IsStunned())
        {
            return;
        }
        if (dashTimer < 0)
            StopDash();

        if (!dashing)
        {
            timeSinceDashEnd += Time.fixedDeltaTime;
            float followT = followDist / maxFollowDist;
            float followMult = followCurve.Evaluate(followT);
            body.velocity = followMult * movementSpeed * movementVector;
        }
        else
        {
            dashTimer -= Time.fixedDeltaTime;
            float t = 1f - (dashTimer / dashTime);
            float speed = dashSpeed * dashSpeedCurve.Evaluate(t);
            body.velocity = speed * dashDirection;
        }

        animator.SetFloat("speed", Mathf.Abs(body.velocity.magnitude));
    }
}
