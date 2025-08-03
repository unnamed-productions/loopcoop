using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public enum EnemyState
    {
        NEUTRAL,
        ATTACKING,
        // enemy is captured by lasso and is frozen
        CAPTURED,
        // enemy is stunned and cannot move (cooldown after attacking/getting attacked)
        STUNNED
    }

    public EnemyState currentState;

    private EnemyState previousState;

    public Rigidbody2D rb;
    // Animator anim;

    [SerializeField]
    public float maxSpeed = 5;

    [SerializeField]
    public float deceleration = 0.1f;

    [SerializeField]
    public float acceleration = 0.3f;

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

    [SerializeField] private float waypointDistance = 3f;

    // [SerializeField]
    // Sound hitSound;

    int curHealth;

    private static Vector2[] cardinalDirs = {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };

    private Vector2 chosenOffset;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curHealth = maxHealth;
        // anim = GetComponent<Animator>();
        // TODO: start in neutral, walk towards player when within range
        currentState = EnemyState.NEUTRAL;
        if (rb.velocity.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (GameManager.instance.currentGameState == GameManager.GameState.PAUSED)
        // {
        //     return;
        // }
        switch (currentState)
        {
            case EnemyState.NEUTRAL:
                break;
            case EnemyState.ATTACKING:
                break;
            case EnemyState.STUNNED:
                DecelerateTowardsZero();
                break;
            case EnemyState.CAPTURED:
                break;
        }
    }

    public void MoveTowardsPlayerOffset()
    {
        // compute the actual point we’re heading for
        Vector2 playerPos = GameManager.instance.GetPlayer().GetPosition();
        Vector2 targetPos = playerPos + chosenOffset;

        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;
        rb.velocity = dir * maxSpeed;

        // optional: flip sprite
        var sr = GetComponent<SpriteRenderer>();
        if (sr) sr.flipX = dir.x < 0;
    }


    public void DecelerateTowardsZero()
    {
        rb.velocity -= rb.velocity.normalized * deceleration;
        if (rb.velocity.magnitude <= deceleration)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    public void Stun(float time)
    {
        if (currentState != EnemyState.STUNNED) previousState = currentState;
        currentState = EnemyState.STUNNED;

        Invoke("Unstun", time);
    }

    private void Unstun()
    {
        currentState = previousState;
    }

    private void Capture()
    {
        if (previousState != EnemyState.STUNNED) previousState = currentState;
        currentState = EnemyState.CAPTURED;

        // TODO lasso swing setup logic
    }

    private void Release()
    {
        currentState = previousState;

        // TODO lasso swing release logic
        // do we immediately kill the enemy after lassoing it? or just do some damage
    }

    public bool CanAccelerate()
    {
        return rb.velocity.magnitude < maxSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("collision entered");
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name == "Player")
        {
            Debug.Log("player collision");
            Rigidbody2D playerRB = other.rigidbody;
            // apply pushback force on player and enemy in direction of collision
            Vector2 playerPushForce = (other.transform.position - transform.position).normalized * force;
            Vector2 enemyPushForce = (transform.position - other.transform.position).normalized * force;

            rb.AddForce(enemyPushForce, ForceMode2D.Impulse);
            GameManager.instance.GetPlayer().Hit(contactDamage, 0.2f, playerPushForce);


            Stun(0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int idx = Random.Range(0, cardinalDirs.Length);
            chosenOffset = cardinalDirs[idx] * waypointDistance;

            currentState = EnemyState.ATTACKING;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "TopDownPlayer")
        {
            currentState = EnemyState.NEUTRAL;
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

    public bool IsAggro()
    {
        return currentState == EnemyState.ATTACKING;
    }

    public bool IsCaptured()
    {
        return currentState == EnemyState.CAPTURED;
    }

    public void UpdateMaxSpeed(float newVal)
    {
        maxSpeed = newVal;
    }

    public float GetAcceleration()
    {
        return acceleration;
    }

    public Vector2 GetVectorToPlayer()
    {
        if (!GameManager.instance) return Vector2.zero;
        Vector2 playerPos = GameManager.instance.GetPlayer().GetPosition();
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);
        return playerPos - enemyPos;
    }
}
