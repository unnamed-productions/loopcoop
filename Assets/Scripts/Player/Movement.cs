using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    float speedX;
    float speedY;
    Rigidbody2D rb;
    TopDownPlayer state;
    [SerializeField]
    float deceleration = 0.1f;

    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        state = GetComponent<TopDownPlayer>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState != GameManager.GameState.PAUSED)
        {
            speedX = InputManager.instance.moveInput.x * moveSpeed;
            speedY = InputManager.instance.moveInput.y * moveSpeed;

            switch (state.GetState())
            {
                case TopDownPlayer.PlayerState.Default:
                    rb.velocity = new Vector2(speedX, speedY);
                    state.SetDirection(speedX, speedY);
                    string anim = "Idle";
                    string dirn = "Left";
                    if (rb.velocity.magnitude > 0)
                    {
                        anim = "Run";
                    }
                    switch (state.GetDirection())
                    {
                        case TopDownPlayer.PlayerDirection.Up:
                            dirn = "Up";
                            sprite.flipX = false;
                            break;
                        case TopDownPlayer.PlayerDirection.Down:
                            dirn = "Down";
                            sprite.flipX = false;
                            break;
                        case TopDownPlayer.PlayerDirection.Left:
                            dirn = "Right"; // not a typo, assumes player will be flipping
                            sprite.flipX = true;
                            break;
                        case TopDownPlayer.PlayerDirection.Right:
                            dirn = "Right";
                            sprite.flipX = false;
                            break;
                    }

                    state.UpdateAnimationState(anim + dirn);
                    break;
                case TopDownPlayer.PlayerState.Stunned:
                    if (rb.velocity.magnitude <= deceleration)
                    {
                        rb.velocity = Vector2.zero;
                    }
                    else
                    {
                        rb.velocity -= (rb.velocity.normalized) * deceleration;
                    }
                    break;
                case TopDownPlayer.PlayerState.Attack:
                    if (rb.velocity.magnitude <= deceleration)
                    {
                        rb.velocity = Vector2.zero;
                    }
                    else
                    {
                        rb.velocity -= (rb.velocity.normalized) * deceleration;
                    }
                    break;
            }
        }
    }
}
