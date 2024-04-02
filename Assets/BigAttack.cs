using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAttack : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("Hit");
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        
    }


}
