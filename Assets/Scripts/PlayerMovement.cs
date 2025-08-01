using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //basic movement
    public float movementSpeed;
    public float turnSpeed;
    public AnimationCurve followCurve;
    public float maxFollowDist;
    private float followDist;
    private bool facingRight = false;
    private Vector2 movementVector = Vector2.zero;

    //dash
    public float dashSpeed;
    public AnimationCurve dashSpeedCurve;
    public float maxDashTime;
    private bool dashing = false;
    private Vector2 dashDirection = Vector2.zero;
    private float dashTime = 0f;


    //components
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
    
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector2 mouseWorldPoint = (Vector2)Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 playerWorldPoint = (Vector2)transform.position;
        Vector2 mouseDist = mouseWorldPoint - playerWorldPoint;
        Vector2 newMovementVector = mouseDist.normalized;

        if (Input.GetMouseButtonDown(0) && !dashing) {
            dashing = true;
            dashTime = maxDashTime;
            dashDirection = newMovementVector;
        }

        followDist = Mathf.Min(mouseDist.magnitude, maxFollowDist);
        float t = Time.deltaTime;
        movementVector = Vector2.Lerp(movementVector, newMovementVector, t*turnSpeed);

        facingRight = movementVector.x > 0;
        float scalar = Mathf.Sign(facingRight ? -1f : 1f) * Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(scalar, transform.localScale.y, transform.localScale.z);
    }

    void FixedUpdate(){
        
        if (dashing) {
            dashTime -= Time.fixedDeltaTime;
            if (dashTime <= 0f) dashing = false;
            else
            {
                float t = 1f - (dashTime / maxDashTime);
                float currDashSpeed = dashSpeed * dashSpeedCurve.Evaluate(t);
                body.velocity = currDashSpeed * dashDirection;
                return;
            }
        }

        float followMult = followCurve.Evaluate(followDist / maxFollowDist);
        body.velocity = movementVector * movementSpeed * followMult;
    }
}
