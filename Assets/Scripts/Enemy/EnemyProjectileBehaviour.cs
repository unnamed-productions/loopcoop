using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyBehaviour enemyState;
    private Rigidbody2D rb;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 5f;
    [Tooltip("How close to attackRange before we consider ourselves 'in range'")]
    [SerializeField] private float rangeTolerance = 0.2f;
    [SerializeField] private float attackMoveSpeed = 2f;
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float projectileSpreadDeg = 5f;

    [SerializeField]
    Projectile projectile;

    [SerializeField]
    float projectileSpreadDegrees = 5f;

    private bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyState.currentState != EnemyBehaviour.EnemyState.ATTACKING)
            return;

        Vector2 toPlayer = enemyState.GetVectorToPlayer();
        float dist = toPlayer.magnitude;
        Vector2 dirNorm = toPlayer.normalized;

        if (dist > attackRange + rangeTolerance)
        {
            rb.velocity = dirNorm * attackMoveSpeed;
        }
        else if (dist < attackRange - rangeTolerance)
        {
            rb.velocity = -dirNorm * attackMoveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;

            if (canShoot)
                ShootProjectile();
        }
    }

    public void ShootProjectile()
    {
        canShoot = false;
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
        Invoke(nameof(EnableProjectile), attackCooldown);
    }

    private void EnableProjectile()
    {
        canShoot = true;
    }
}
