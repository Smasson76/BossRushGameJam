using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAnimation : MonoBehaviour
{
    // Start is called before the first frame update

   public float animSpeed = 1f;
    Animator m_Animator;
      
    void Start()
    {
        m_Animator = this.gameObject.GetComponent<Animator>();
        m_Animator.speed = animSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
