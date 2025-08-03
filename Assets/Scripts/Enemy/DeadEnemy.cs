using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DeadEnemy : MonoBehaviour
{
    [SerializeField]
    Vector2 vertForceToAdd; // Use x as min and y as max upward force
    [SerializeField]
    Vector2 horizForceToAddRange; // Use x as min and y as max horizontal force
    [SerializeField]
    float rotationalForceToAdd;

    [SerializeField]
    float lifetime = 5;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Random horizontal force (left or right)
        float horizForce = Random.Range(horizForceToAddRange.x, horizForceToAddRange.y);
        horizForce *= Random.value < 0.5f ? -1 : 1;

        // Random vertical force (always up)
        float vertForce = Random.Range(vertForceToAdd.x, vertForceToAdd.y);

        // Apply force
        rb.AddForce(new Vector2(horizForce, vertForce), ForceMode2D.Impulse);

        // Apply torque
        float torque = Random.Range(-rotationalForceToAdd, rotationalForceToAdd);
        rb.AddTorque(torque, ForceMode2D.Impulse);
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            Destroy(gameObject);
        }
    }
}

