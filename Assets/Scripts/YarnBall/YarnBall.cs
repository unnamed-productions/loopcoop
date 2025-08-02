using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class YarnBall : MonoBehaviour
{
    public Rigidbody2D playerRb;     // Reference to the player's Rigidbody2D
    public float baseTorque = 10f;   // Base rotational force
    public float boostMultiplier = 2f; // How much faster to go when aligned

    private Rigidbody2D rb;

    [SerializeField]
    GameObject linkPrefab;

    bool hasHit = false;

    private List<GameObject> children; //Rope segments

    [SerializeField]
    int maxChainLinks = 20;

    [Header("Smashing")]
    [SerializeField]
    //If velocity is greater than this number, initiate a "piercing" hit
    float VelocityThreshold = 20;
    //Decide how many hits to chain by dividing the difference between our velocity and the threshold by this number
    [SerializeField]
    float VelocityStepSize = 3;
    //Multiply velocity by this number to get camera shake intensity
    [SerializeField]
    float VelocityToShakeMult = .01f;
    //Multiple the camera shake by this number for every consecutive "piercing" hit
    [SerializeField]
    float VelocitychainMult = 1.2f;
    [SerializeField]
    //The max amount of time between piercing hits
    float PiercingSeconds = 1f;
    private float currPiercingSeconds = 0;
    private bool Smacking;
    private int currSmacksRemaining;
    private int currChainSmacks = 1; //The number of targets hit in this chain




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        children = new List<GameObject>();
        init();
    }

    void Update()
    {
        if (Smacking)
        {
            currPiercingSeconds -= Time.deltaTime;
            if (currPiercingSeconds <= 0)
            {
                Dissolve();
            }
        }

    }

    void FixedUpdate()
    {
        if (playerRb == null) return;

        Vector2 toBall = rb.position - playerRb.position;
        Vector2 tangent = new Vector2(-toBall.y, toBall.x).normalized; // Perpendicular (right-hand rule)

        // Get how much the player is moving along the tangent
        float alignment = Vector2.Dot(playerRb.gameObject.GetComponent<PlayerMovement>().getMoveDirection(), tangent);

        // Final force: base + bonus when aligned
        float force = baseTorque;
        if (alignment > 0.1f) // adjust threshold as needed
        {
            force *= Mathf.Lerp(1f, boostMultiplier, alignment); // smoothly boost
        }

        // Apply tangential force to the ball
        rb.AddForce(tangent * force, ForceMode2D.Force);
        //print("Velocity: " + rb.velocity.magnitude);
    }

    private void init()
    {
        GameObject playr = GameObject.FindGameObjectWithTag("Player");
        playerRb = playr.GetComponent<Rigidbody2D>();
        float distToPlayer = Vector2.Distance(transform.position, playr.transform.position);
        int numSegments = (int)(distToPlayer * 1 / linkPrefab.GetComponent<DistanceJoint2D>().distance); //Round to nearest whole number
        numSegments = Math.Clamp(numSegments, 1, maxChainLinks); //Make sure we have at least one segment but not a crazy amount

        GameObject currSegment = gameObject;
        for (int i = 0; i < numSegments; i++)
        {
            GameObject temp = Instantiate(linkPrefab, transform.position, quaternion.identity);
            children.Add(temp);
            currSegment.GetComponent<DistanceJoint2D>().connectedBody = temp.GetComponent<Rigidbody2D>();
            currSegment = temp;
        }
        currSegment.GetComponent<DistanceJoint2D>().connectedBody = playerRb;
        playr.GetComponent<DistanceJoint2D>().connectedBody = currSegment.GetComponent<Rigidbody2D>();
        playr.GetComponent<DistanceJoint2D>().enabled = true;
        GetComponent<DistanceJoint2D>().enabled = true;

        StartCoroutine(stabilize());
    }

    IEnumerator stabilize()
    {
        float tm = 5;
        while (tm < 0)
        {
            tm -= Time.deltaTime;
            //Don't exactly know why this is required but it is
            transform.position = GetComponent<DistanceJoint2D>().connectedBody.transform.position;
            GetComponent<DistanceJoint2D>().connectedAnchor = GetComponent<DistanceJoint2D>().connectedBody.transform.position;
            yield return null;
        }
    }

    public void Smack(YarnBallHittable hit)
    {
        if (!Smacking)
        {
            //First smack
            //print("Velocity mag " + rb.velocity.magnitude);
            currSmacksRemaining = (int)((rb.velocity.magnitude - VelocityThreshold) / VelocityStepSize);
            print("Initiated hit, can hit " + currSmacksRemaining + " extra targets");
            if (currSmacksRemaining >= 1)
            {
                //print("Chain hitting with intensity " + VelocityToShakeMult * rb.velocity.magnitude * (float)Mathf.Pow(VelocitychainMult, currChainSmacks));
                hit.hitMe(VelocityToShakeMult * VelocitychainMult * rb.velocity.magnitude);
                Smacking = true;
                currPiercingSeconds = PiercingSeconds;
            }
            else
            {
                //print("Hitting with intensity " + VelocityToShakeMult * rb.velocity.magnitude);
                hit.hitMe(VelocityToShakeMult * rb.velocity.magnitude);
                Dissolve();
            }
        }
        else
        {
            if (currSmacksRemaining >= 1)
            {
                //TODO: Accumulate points
                //print("Chain hitting with intensity " + VelocityToShakeMult * rb.velocity.magnitude * (float)Mathf.Pow(VelocitychainMult, currChainSmacks));
                hit.hitMe(VelocityToShakeMult * (float)Mathf.Pow(VelocitychainMult, currChainSmacks) * rb.velocity.magnitude);
                currChainSmacks++;
                currSmacksRemaining--;
                if (currSmacksRemaining == 0)
                {
                    Dissolve();
                }
            }
            //Subsequent Smacks
            currPiercingSeconds = PiercingSeconds; //Refresh timer

        }

    }

    private void Dissolve()
    {
        //TODO
        //Play animation, add points, make particles, kinda do whatever
        playerRb.gameObject.GetComponent<DistanceJoint2D>().enabled = false; //Free player
        foreach (GameObject g in children)
        {
            Destroy(g);
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<YarnBallHittable>())
        {
            Smack(collision.gameObject.GetComponent<YarnBallHittable>());
        }
    }
}
