using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Animation WalkAnimation_01;
    private Animation WalkAnimation_02;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        WalkAnimation_01 = transform.GetChild(2).GetComponent<Animation>();
        WalkAnimation_02 = transform.GetChild(3).GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        WalkAnimation_01.Play();
        WalkAnimation_02.Play();
    }
}
