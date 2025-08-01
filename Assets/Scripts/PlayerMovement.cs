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
    private bool facingRight = false;
    private float dashTime = 0f;

    void Update()
    {

        float deltaTime = Time.deltaTime;

        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector2 mouseWorldPoint = (Vector2)Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 playerWorldPoint = (Vector2)transform.position;
        Vector2 movementVector = mouseWorldPoint - playerWorldPoint;
        facingRight = movementVector.x > 0;

        //handle all dash logic
        if (dashing)
        {

            dashTime -= deltaTime;
            if (dashTime < 0f)
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
