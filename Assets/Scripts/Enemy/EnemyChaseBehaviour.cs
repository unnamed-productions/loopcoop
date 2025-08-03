using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseBehaviour : MonoBehaviour
{
    [SerializeField]
    EnemyBehaviour enemyState;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState.currentState)
        {
            case EnemyBehaviour.EnemyState.NEUTRAL:
                break;
            case EnemyBehaviour.EnemyState.ATTACKING:
                PursuePlayer();
                break;
            case EnemyBehaviour.EnemyState.STUNNED:
                break;
            case EnemyBehaviour.EnemyState.CAPTURED:
                break;
        }
    }

    public void PursuePlayer()
    {
        if (rb.velocity.magnitude > enemyState.maxSpeed)
        {
            rb.velocity -= rb.velocity.normalized * enemyState.deceleration;
        }
        else
        {
            enemyState.MoveTowardsPlayer();
        }
    }
}
