using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPlayer : MonoBehaviour
{
    public enum PlayerState {Default, Attack, Stunned}
    public enum PlayerDirection{Left, Right, Up, Down}
    private PlayerState currentState;
    private PlayerDirection currentDirection;
    private int curHealth;
    [SerializeField]
    private int maxHealth = 3;
    Rigidbody2D rb;

    // The time to be invulnerable after getting hit
    private float invulnTime;

    private bool invulnerable;
    private string currentAnimationState;

    Animator animator;

    [SerializeField]
    CameraShake cs;

    [SerializeField]
    Sound hitSound;

    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.Default;
        currentDirection = PlayerDirection.Left;
        curHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        currentAnimationState = "";
        animator = GetComponent<Animator>();
    }

    public void Reset() {
        currentState = PlayerState.Default;
        currentDirection = PlayerDirection.Left;
        curHealth = maxHealth;
        currentAnimationState = "";
        transform.localPosition = new Vector3(0, 0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetPosition() {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public void Default()
    {
        currentState = PlayerState.Default;
    }

    public void Attack()
    {
        currentState = PlayerState.Attack;
    }

    public PlayerState GetState()
    {
        return currentState;
    }

    public PlayerDirection GetDirection()
    {
        return currentDirection;
    }

    public void SetDirection(float xVel, float yVel)
    {
        if(xVel < 0) currentDirection = PlayerDirection.Left;
        else if(xVel > 0) currentDirection = PlayerDirection.Right;
        else if(yVel < 0) currentDirection = PlayerDirection.Down;
        else if(yVel > 0) currentDirection = PlayerDirection.Up;
    }

    public Vector2 DirectionToVec() {
        switch(currentDirection) {
            case PlayerDirection.Left:
                return new Vector2(-1, 0);
            case PlayerDirection.Right:
                return new Vector2(1, 0);
            case PlayerDirection.Up:
                return new Vector2(0, 1);
            case PlayerDirection.Down:
                return new Vector2(0, -1);
            default:
                return new Vector2(1, 0);
        }
    }

    public void Hit(int damage, float stunTime, Vector2 knockbackForce) {
        if(!invulnerable){
            cs.ShakeCamera(.5f, .05f);
            curHealth -= damage;
            AudioManager.instance.PlaySound(hitSound, transform);
            if(curHealth <= 0){
                //TODO: Have the player die
                return;
            }

            rb.AddForce(knockbackForce, ForceMode2D.Impulse);
            if(stunTime > 0) Stun(stunTime);
            MakeInvulnerable(stunTime * 2);
            myHUD.SetHearts(curHealth);
        }
    }

    public bool IsStunned() {
        return currentState == PlayerState.Stunned;
    }

    public void Stun(float time) {
        currentState = PlayerState.Stunned;
        Invoke("Unstun", time);
    }

    public void Unstun() {
        currentState = PlayerState.Default;
    }

    public void MakeInvulnerable(float time) {
        invulnerable = true;
        Invoke("MakeVulnerable", time);
    }

    public void MakeVulnerable() {
        invulnerable = false;
    }

    public void Knockback(Vector2 knockbackForce){
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
    }

    public void UpdateAnimationState(string newState){
        if(currentAnimationState == newState) return;
        animator.Play(newState);
        currentAnimationState = newState;
    }
}
