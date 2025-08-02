using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    EnemyBehaviour enemyState;

    [SerializeField]
    readonly float attackRange = 5f;

    [SerializeField]
    readonly float attackCooldown = 2f;
    public Rigidbody2D rb;

    [SerializeField]
    Projectile projectile;

    bool projectileEnabled = true;

    [SerializeField]
    float projectileSpreadDegrees = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        projectileEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState.currentState)
        {
            case EnemyBehaviour.EnemyState.NEUTRAL:
                break;
            case EnemyBehaviour.EnemyState.ATTACKING:
                enemyState.DecelerateTowardsZero();
                if (projectileEnabled) ShootProjectile();
                break;
            case EnemyBehaviour.EnemyState.STUNNED:
                break;
            case EnemyBehaviour.EnemyState.CAPTURED:
                break;
        }
    }

    public void ShootProjectile()
    {
        const float DELTA = 1f;

        Vector2 dirn = enemyState.GetVectorToPlayer().normalized;
        Projectile p = Instantiate(projectile, transform.position
             + new Vector3(dirn.x, dirn.y, 0) * DELTA, transform.rotation);
        p.Initialize(dirn, 1, transform);
        p.transform.SetParent(transform.parent);

        Vector2 left = Quaternion.Euler(0, 0, -projectileSpreadDegrees) * dirn;
        Projectile p2 = Instantiate(projectile, transform.position
             + new Vector3(left.x, left.y, 0) * DELTA, transform.rotation);
        p2.Initialize(left, 1, transform);
        p2.transform.SetParent(transform.parent);

        Vector2 right = Quaternion.Euler(0, 0, projectileSpreadDegrees) * dirn;
        Projectile p3 = Instantiate(projectile, transform.position
             + new Vector3(right.x, right.y, 0) * DELTA, transform.rotation);
        p3.Initialize(right, 1, transform);
        p3.transform.SetParent(transform.parent);


        // AudioManager.instance.PlaySound(sendEmailSound, transform);
        DisableProjectile();
    }

    private void DisableProjectile()
    {
        projectileEnabled = false;

        Invoke("EnableProjectile", attackCooldown);
    }

    private void EnableProjectile()
    {
        projectileEnabled = true;
    }
}
