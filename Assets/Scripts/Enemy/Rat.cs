using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    YarnTrail yarnTrail;

    [SerializeField]
    float movementSpeed = 3f;

    Rigidbody2D rb;

    EnemyBehaviour enemyState;

    [SerializeField]
    float yarnDigestionTime = 0.5f;

    int targetYarnIdx;

    bool isDigesting;

    // Start is called before the first frame update
    void Start()
    {
        yarnTrail = player.GetComponent<YarnTrail>();
        rb = GetComponent<Rigidbody2D>();
        enemyState = GetComponent<EnemyBehaviour>();
        targetYarnIdx = GetClosestYarnLink();
        isDigesting = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState.currentState)
        {
            case EnemyBehaviour.EnemyState.NEUTRAL:
                List<GameObject> yarnSegments = yarnTrail.GetYarnSegments();
                if (yarnSegments.Count > 0)
                {
                    targetYarnIdx = GetClosestYarnLink();
                    enemyState.currentState = EnemyBehaviour.EnemyState.ATTACKING;
                }
                rb.velocity = Vector2.zero;
                break;
            case EnemyBehaviour.EnemyState.ATTACKING:
                if (targetYarnIdx != -1 && !isDigesting) MoveTowardsYarn();
                else if(!isDigesting) enemyState.currentState = EnemyBehaviour.EnemyState.NEUTRAL;
                else rb.velocity = Vector2.zero;
                break;
            case EnemyBehaviour.EnemyState.STUNNED:
                break;
            case EnemyBehaviour.EnemyState.CAPTURED:
                break;
        }
    }

    int GetClosestYarnLink()
    {
        int idx = -1;
        float minDist = 0;
        List<GameObject> yarnSegments = yarnTrail.GetYarnSegments();
        for (int i = 0; i < yarnSegments.Count; i++)
        {
            float dist = (transform.position - yarnSegments[i].transform.position).magnitude;
            if (idx == -1 || dist < minDist)
            {
                idx = i;
                minDist = dist;
            }
        }
        return idx;
    }

    public void MoveTowardsYarn()
    {
        List<GameObject> yarnSegments = yarnTrail.GetYarnSegments();
        if (targetYarnIdx >= yarnSegments.Count)
        {
            int closest = GetClosestYarnLink();
            if (closest == -1) return;
            targetYarnIdx = closest;
        }

        Vector2 dirn = (yarnSegments[targetYarnIdx].transform.position - transform.position).normalized;
        rb.velocity = dirn * movementSpeed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name.StartsWith("YarnSegment") && !isDigesting)
        {
            List<GameObject> yarnSegments = yarnTrail.GetYarnSegments();
            if (targetYarnIdx < yarnSegments.Count && yarnSegments[targetYarnIdx] == other.gameObject)
            {
                yarnTrail.ClearTrailFrom(targetYarnIdx);
                DangImFullNowAndIGottaDigestThisYarnForABit();
            }
        }
    }

    public void DangImFullNowAndIGottaDigestThisYarnForABit()
    {
        Invoke("WowImHungryAndReadyToEatSomeMoreYarn", yarnDigestionTime);
        isDigesting = true;
    }

    private void WowImHungryAndReadyToEatSomeMoreYarn()
    {
        targetYarnIdx = 0;
        isDigesting = false;
    }
}
