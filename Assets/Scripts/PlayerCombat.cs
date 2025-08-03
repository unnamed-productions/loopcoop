using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    int curHealth = 10;

    private bool isStunned = false;
    private bool invulnerable = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Hit(int damage, float stunTime, Vector2 knockbackForce)
    {
        if (!invulnerable)
        {
            curHealth -= damage;
            // AudioManager.instance.PlaySound(hitSound, transform);
            if (curHealth <= 0)
            {
                //TODO: Have the player die
                return;
            }
            Debug.Log("add force to player");
            rb.AddForce(knockbackForce, ForceMode2D.Impulse);
            if (stunTime > 0) Stun(stunTime);
            MakeInvulnerable(stunTime * 2);
        }
    }

    public void MakeInvulnerable(float time)
    {
        invulnerable = true;
        Invoke("MakeVulnerable", time);
    }

    public void MakeVulnerable()
    {
        invulnerable = false;
    }

    public void Stun(float time)
    {
        isStunned = true;
        Invoke("Unstun", time);
    }

    public void Unstun()
    {
        isStunned = false;
    }

    public bool IsStunned()
    {
        return isStunned;
    }

    public Vector2 GetPosition() {
        return new Vector2(transform.position.x, transform.position.y);
    }
}
