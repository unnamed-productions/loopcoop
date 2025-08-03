using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    Rigidbody2D rb;

    const float FIREBALL_SPEED = 8f;

    [SerializeField]
    int damage = 2;
    [SerializeField]
    float knockbackForce = 15f;

    [SerializeField]
    Animator animator;

    [SerializeField]
    bool shouldFlip;

    [SerializeField]
    SpriteRenderer sprite;

    [SerializeField]
    float maxRange = 50f;

    Vector2 startPosition;

    Transform owner;

    [SerializeField]
    List<Sprite> possibleSprites;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        int rand = Random.Range(0, possibleSprites.Count);
        GetComponent<SpriteRenderer>().sprite = possibleSprites[rand];
    }

    public void Initialize(Vector2 dirn, int damage, Transform owner)
    {
        direction = new Vector2(dirn.x, dirn.y);
        float angle = Mathf.Atan2(dirn.y, dirn.x) * Mathf.Rad2Deg;
        if (shouldFlip && Mathf.Abs(angle) > 90)
        {
            sprite.flipY = true;
        }
        Debug.Log("Angle:" + angle);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        this.damage = damage;
        this.owner = owner;
        // animator.Play("Default");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState != GameManager.GameState.PAUSED)
        {
            rb.velocity = direction * FIREBALL_SPEED;
        }

        if ((rb.position - startPosition).magnitude > maxRange)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == owner) return;

        // if collision is player, call function to deal damage
        if (other.gameObject.name == "Player")
        {
            TopDownPlayer player = other.gameObject.GetComponent<TopDownPlayer>();
            // player.Hit(damage, 0.2f, direction.normalized * knockbackForce);
            Destroy(gameObject);
        }

        // TODO: if it hits yarn, just destroy it

        // Destroy(gameObject);

    }
}
