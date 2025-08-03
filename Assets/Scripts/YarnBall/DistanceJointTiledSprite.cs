using UnityEngine;
using System.Collections.Generic;

public class DistanceJointTiledSprite : MonoBehaviour
{
    public DistanceJoint2D joint;
    public GameObject segmentPrefab; // A prefab with a SpriteRenderer, 1 unit wide
    public float segmentSpacing = 0.2f;

    private List<Transform> segments = new();

    void LateUpdate()
    {
        if (joint == null || joint.connectedBody == null || segmentPrefab == null)
            return;

        Vector3 anchorA = joint.transform.TransformPoint(joint.anchor);
        Vector3 anchorB = joint.connectedBody.transform.TransformPoint(joint.connectedAnchor);
        Vector3 delta = anchorB - anchorA;
        float distance = delta.magnitude;
        int neededSegments = Mathf.CeilToInt(distance / segmentSpacing);

        // Create or remove segments as needed
        while (segments.Count < neededSegments)
        {
            GameObject seg = Instantiate(segmentPrefab, transform);
            seg.transform.localScale = new Vector3(
                seg.transform.localScale.x / transform.localScale.x,
                seg.transform.localScale.y / transform.localScale.y,
                seg.transform.localScale.z / transform.localScale.z);
            segments.Add(seg.transform);
        }
        while (segments.Count > neededSegments)
        {
            Destroy(segments[^1].gameObject);
            segments.RemoveAt(segments.Count - 1);
        }

        // Position and rotate each segment
        for (int i = 0; i < segments.Count; i++)
        {
            float t = (i + 0.5f) / neededSegments;
            Vector3 pos = Vector3.Lerp(anchorA, anchorB, t);
            segments[i].position = pos;
            segments[i].right = delta.normalized;
        }
    }
}
