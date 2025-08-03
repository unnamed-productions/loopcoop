using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMimic : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer toMimic;
    private SpriteRenderer mySR;
    // Start is called before the first frame update
    void Start()
    {
        mySR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mySR.sprite = toMimic.sprite;
        transform.localScale = toMimic.transform.localScale;
        mySR.flipX = toMimic.flipX;
        MatchGlobalScale(transform, toMimic.transform);
    }

    void MatchGlobalScale(Transform target, Transform toMimic)
    {
        Vector3 parentScale = target.parent ? target.parent.lossyScale : Vector3.one;
        Vector3 desiredGlobalScale = toMimic.lossyScale;

        // Avoid division by zero
        Vector3 newLocalScale = new Vector3(
            parentScale.x != 0 ? desiredGlobalScale.x / parentScale.x : 0,
            parentScale.y != 0 ? desiredGlobalScale.y / parentScale.y : 0,
            parentScale.z != 0 ? desiredGlobalScale.z / parentScale.z : 0
        );

        target.localScale = newLocalScale;
    }
}
