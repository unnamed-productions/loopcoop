using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathAnimation : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    void Start()
    {
    }

    public void PlayAnimation()
    {
        Debug.Log("playing death anim");
        // TODO: play death animation
    }

    void Update()
    {
        
    }

    public void QueueDelete(float len){
        Invoke("DestroyAnimation", len);
    }

    void DestroyAnimation(){
        Destroy(gameObject);
    }
}
