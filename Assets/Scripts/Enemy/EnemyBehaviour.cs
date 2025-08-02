using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public enum EnemyState
    {
        NEUTRAL,
        WALKING_TOWARDS_PLAYER,
        ATTACKING,
        // enemy is captured by lasso and is frozen
        CAPTURED,
        // enemy is stunned and cannot move (cooldown after attacking/getting attacked)
        STUNNED
    }

    private EnemyState currentState;

    Rigidbody2D rb;
    Animator anim;

    [SerializeField]
    float maxSpeed = 5;

    [SerializeField]
    float deceleration = 0.1f;

    [SerializeField]
    float acceleration = 0.3f;

    [SerializeField]
    float force = 10;

    [SerializeField]
    int contactDamage = 1;

    [SerializeField]
    int maxHealth = 3;

    [SerializeField]
    float stunTime = 0.4f;

    [SerializeField]
    Sound enemyDeathSound;

    [SerializeField]
    EnemyDeathAnimation enemyDeathAnimation;

    // [SerializeField]
    // Sound hitSound;

    int curHealth;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curHealth = maxHealth;
        anim = GetComponent<Animator>();
        // TODO: start in neutral, walk towards player when within range
        currentState = EnemyState.WALKING_TOWARDS_PLAYER;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameManager.GameState.PAUSED)
        {
            return;
        }
        switch (currentState)
        {
            case EnemyState.NEUTRAL:
                rb.velocity = Vector3.zero;
                break;
            case EnemyState.WALKING_TOWARDS_PLAYER:
                if (rb.velocity.magnitude > maxSpeed)
                {
                    rb.velocity -= rb.velocity.normalized * deceleration;
                }
                else
                {
                    MoveTowardsPlayer();
                }
                break;
            case EnemyState.ATTACKING:
                break;
            case EnemyState.STUNNED:
                break;
            case EnemyState.CAPTURED:
                break;
        }
    }

    public void MoveTowardsPlayer(){
        Vector2 playerPos = GameManager.instance.GetPlayer().GetPosition();
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 dirn = (playerPos - enemyPos).normalized;
        if (dirn.x > 0)
        {
            anim.SetBool("Left", false);
        }
        else
        {
            anim.SetBool("Left", true);
        }
        rb.velocity += dirn * acceleration;
    }

    public void DecelerateTowardsZero(){
        rb.velocity -= (rb.velocity.normalized * deceleration);
        if(rb.velocity.magnitude <= deceleration){
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void Stun(float time){
        currentState = EnemyState.STUNNED;

        Invoke("Unstun", time);
    }

    private void Unstun(){
        currentState = EnemyState.WALKING_TOWARDS_PLAYER;
    }

    private void Capture()
    {
        currentState = EnemyState.CAPTURED;

        // TODO lasso swing setup logic
    }

    private void Release()
    {
        currentState = EnemyState.WALKING_TOWARDS_PLAYER;

        // TODO lasso swing release logic
        // do we immediately kill the enemy after lassoing it? or just do some damage
    }

    public bool CanAccelerate() {
        return rb.velocity.magnitude < maxSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.name == "TopDownPlayer") {
            Rigidbody2D playerRB = other.rigidbody;
            // apply pushback force on player and enemy in direction of collision
            Vector2 playerPushForce = (other.transform.position - transform.position).normalized * force;
            Vector2 enemyPushForce = (transform.position - other.transform.position).normalized * force;

            rb.AddForce(enemyPushForce, ForceMode2D.Impulse);
            GameManager.instance.GetPlayer().Hit(contactDamage, 0.2f, playerPushForce);


            Stun(0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name == "TopDownPlayer") {
            currentState = EnemyState.ATTACKING;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.name == "TopDownPlayer") {
            currentState = EnemyState.WALKING_TOWARDS_PLAYER;
        }
    }

    /**
    called when enemy gets hit by lasso

    if enemy takes damage while in the lasso, probably just call this without any knockbackForce
    */
    public void Hit(int damage, Vector2 knockbackForce)
    {
        curHealth -= damage;
        if (curHealth <= 0)
        {
            GameManager.instance.score += 1;

            // TODO: decrement enemy count
            // TODO: play death animation
            // TODO: play death sound
            Destroy(gameObject);
            return;
        }

        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        Stun(stunTime);
    }

    public bool IsAggro(){
        return currentState == EnemyState.ATTACKING;
    }

    public bool IsCaptured(){
        return currentState == EnemyState.CAPTURED;
    }

    public void UpdateMaxSpeed(float newVal){
        maxSpeed = newVal;
    }

    public float GetAcceleration(){
        return acceleration;
    }
}
