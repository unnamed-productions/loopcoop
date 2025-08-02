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
    public float dashTime = 0.5f;
    private bool dashing = false;
    private Vector2 dashDirection = Vector2.zero;
    [SerializeField] private float dashTimer = 0f;

    //chain dash
    public Vector2 chainDashTriggerInterval = new(0.75f, 1.0f);  

    //dash cooldown
    public float failedDashCooldownTime = 2;
    [SerializeField] private float failedDashCooldownTimer;

    //dash UI 
    public TextMeshProUGUI dashCounter;
    private int dashCount = 0;


    //components
    private Rigidbody2D body;
    private SpriteRenderer sprite;

     void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        dashCounter.enabled = false;
    }

    void StartDash(Vector2 direction)
    {
        dashing = true;
        dashTimer = dashTime;
        dashDirection = direction;
        dashCount++;

        dashCounter.text = dashCount.ToString();
        dashCounter.enabled = true;

    }

    void StopDash()
    {
        dashing = false;
        dashTimer = 0f;
        failedDashCooldownTimer = failedDashCooldownTime;

        body.velocity = Vector2.zero;

        dashCount = 0;
        dashCounter.enabled = false;
    }

    void Update()
    {

        Vector2 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = mouseWorldPoint - (Vector2)transform.position;
        Vector2 newMovementVector = toMouse.normalized;

        if (Input.GetMouseButtonDown(0))
        {
            float dashProgress = 1f - (dashTimer / dashTime);

            if (dashing)
            {
                bool canChain = dashProgress >= chainDashTriggerInterval.x && dashProgress <= chainDashTriggerInterval.y;

                if (canChain)
                {
                    StartDash(newMovementVector);
                }
                else
                {
                    StopDash();
                }
            }
            else if (failedDashCooldownTimer <= 0)
            {
                StartDash(newMovementVector);
            }
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
        if (dashing)
        {
            dashTimer -= Time.fixedDeltaTime;

            if (dashTimer <= 0f)
            {
                StopDash();
            }
            else
            {
                float t = 1f - (dashTimer / dashTime);
                float speed = dashSpeed * dashSpeedCurve.Evaluate(t);
                body.velocity = speed * dashDirection;
            }
        }
        else
        {
            failedDashCooldownTimer -= Time.fixedDeltaTime;

            float followT = followDist / maxFollowDist;
            float followMult = followCurve.Evaluate(followT);

            body.velocity = movementVector * movementSpeed * followMult;
        }
    }
}