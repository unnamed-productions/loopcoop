using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    Rigidbody2D rb;


    [SerializeField]
    int curHealth = 5;
    int maxHealth = 5;

    private bool isStunned = false;
    private bool invulnerable = false;

    public List<Image> hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    public void Hit(int damage, float stunTime, Vector2 knockbackForce)
    {

        if (!invulnerable)
        {
           
            curHealth -= damage;

            for(int i = 0; i < maxHealth; i++) {
                if (i >= curHealth)
                {
                    hearts[i].sprite = emptyHeart;
                }
                else
                {
                    hearts[i].sprite = fullHeart;
                }
            }

            // AudioManager.instance.PlaySound(hitSound, transform);
            if (curHealth < 1)
            {
                GameManager.instance.ToggleGameOver();
                return;
            }
      
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
