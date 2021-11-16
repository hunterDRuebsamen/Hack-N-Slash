using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_run : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    EnemyBehavior eb;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindWithTag("Player").transform;
       rb = animator.GetComponent<Rigidbody2D>();
       eb = animator.GetComponent<EnemyBehavior>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // not in attack range, move toward player
        Vector2 newPos = Vector2.MoveTowards(rb.position, player.position, eb.speed*Time.deltaTime);
        rb.MovePosition(newPos);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}
