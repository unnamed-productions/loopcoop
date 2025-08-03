using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleBehavior : MonoBehaviour
{
    private Vector2 idleWalkDirection;

    float minIdleCooldown = 0f;

    float maxIdleCooldown = 4f;

    private bool isWalking;

    [SerializeField]
    float idleWalkSpeed = 0.4f;

    [SerializeField]
    private EnemyBehaviour enemyState;

    private Rigidbody2D rb;

    [SerializeField]
    bool biasedTowardsPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        idleWalkDirection = Random.insideUnitCircle.normalized;
        ToggleWalk();
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState.currentState)
        {
            case EnemyBehaviour.EnemyState.NEUTRAL:
                if (isWalking)
                {
                    rb.velocity = idleWalkSpeed * idleWalkDirection;
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
                break;
            default:
                break;
        }
    }

    void ToggleWalk()
    {
        isWalking = true;

        if (biasedTowardsPlayer)
        {
            Vector2 vecToPlayer = enemyState.GetVectorToPlayer();
            Debug.Log(vecToPlayer);
            float angle = Random.Range(-90f, 90f);
            idleWalkDirection = Quaternion.Euler(0, 0, angle) * vecToPlayer;
        }
        else
        {
            idleWalkDirection = Random.insideUnitCircle.normalized;
        }
        float walkTime = Random.Range(minIdleCooldown, maxIdleCooldown);

        Invoke("ToggleStand", walkTime);
    }

    void ToggleStand()
    {
        isWalking = false;
        float standTime = Random.Range(minIdleCooldown, maxIdleCooldown);
        Invoke("ToggleWalk", standTime);
    }
}
