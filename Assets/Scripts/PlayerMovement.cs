using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float minMovementDist;
    public float movementSpeed;
    public float dashSpeed;
    public AnimationCurve dashSpeedCurve;
    public float maxDashTime;

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

        float deltaTime = Time.deltaTime;

        if (movementVector.x != 0)
        {
            facingRight = movementVector.x > 0;
            sprite.flipX = !facingRight;
        }
    }

        //handle all dash logic
        if (dashing)
        {

            dashTime -= deltaTime;
            if (dashTime< 0f)
            {
                dashing = false;
            }

float t = 1f - (dashTime / maxDashTime);
float currDashSpeed = dashSpeed * dashSpeedCurve.Evaluate(t);
transform.Translate(currDashSpeed * dashDirection);

        }
        //handle normal movement logic
        else if (movementVector.magnitude > minMovementDist)
{
    transform.Translate(movementVector.normalized * movementSpeed);
}

if (Input.GetMouseButtonDown(0) && !dashing && movementVector.magnitude > minMovementDist)
{

    dashing = true;
    dashTime = maxDashTime;
    dashDirection = movementVector.normalized;


}

//temp: flip scale 
float scalar = Mathf.Sign(facingRight ? -1f : 1f) * Mathf.Abs(transform.localScale.x);
transform.localScale = new Vector3(scalar, transform.localScale.y, transform.localScale.z);
    }
}
