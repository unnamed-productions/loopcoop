using UnityEngine;
using System.Collections;

public class FoliageShake : MonoBehaviour
{
    public AnimationCurve curve;
    public float animationTime = 1f;
   IEnumerator Bounce() {
    Vector3 initScale = transform.localScale;
    float elapsed = 0f;
    while(elapsed < animationTime) {
        transform.localScale = new(initScale.x,
        initScale.y*curve.Evaluate(elapsed/animationTime), 1);
        elapsed += Time.deltaTime;
        yield return null;
    }
    yield return null;
   }

  void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(Bounce());
    }
}
