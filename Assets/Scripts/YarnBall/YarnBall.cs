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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        init();
    }

    void FixedUpdate()
    {
        if (playerRb == null) return;

        Vector2 toBall = rb.position - playerRb.position;
        Vector2 tangent = new Vector2(-toBall.y, toBall.x).normalized; // Perpendicular (right-hand rule)

        print("tangent is " + tangent);
        // Get how much the player is moving along the tangent
        float alignment = Vector2.Dot(playerRb.gameObject.GetComponent<PlayerMovement>().getMoveDirection(), tangent);

        // Final force: base + bonus when aligned
        float force = baseTorque;
        if (alignment > 0.1f) // adjust threshold as needed
        {
            force *= Mathf.Lerp(1f, boostMultiplier, alignment); // smoothly boost
        }
        print("force is " + force);

        // Apply tangential force to the ball
        rb.AddForce(tangent * force, ForceMode2D.Force);
    }

    private void init()
    {
        GameObject playr = GameObject.FindGameObjectWithTag("Player");
        playerRb = playr.GetComponent<Rigidbody2D>();
        float distToPlayer = Vector2.Distance(transform.position, playr.transform.position);
        int numSegments = (int)(distToPlayer * 1 / linkPrefab.GetComponent<DistanceJoint2D>().distance); //Round to nearest whole number
        numSegments = Math.Clamp(numSegments, 1, 30); //Make sure we have at least one segment but not a crazy amount

        GameObject currSegment = gameObject;
        for (int i = 0; i < numSegments; i++)
        {
            GameObject temp = Instantiate(linkPrefab, transform.position, quaternion.identity);
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

    void OnDestroy()
    {
        playerRb.gameObject.GetComponent<DistanceJoint2D>().enabled = false; //Free player
    }

    public void Smack()
    {

    }
}
