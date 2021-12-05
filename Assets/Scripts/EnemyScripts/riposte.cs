using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class riposte : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    EnemyBehavior eb;
    BoxCollider2D hitbox;
    bool hasDamagedPlayer = false;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindWithTag("Player").transform;
       rb = animator.GetComponent<Rigidbody2D>();
       hitbox = animator.GetComponentInChildren<BoxCollider2D>();
       eb = animator.GetComponent<EnemyBehavior>();
       animator.ResetTrigger("riposteFinale");
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, player.position, eb.speed*Time.deltaTime*10);
        rb.MovePosition(newPos);
        if (hitbox.IsTouching(player.GetComponent<CapsuleCollider2D>()) && !hasDamagedPlayer) {
            // the hitbox is touching the player capsule collider, deal damage!
            eb.damagePlayerEvent(EnemyBehavior.AttackType.Melee);
            animator.SetTrigger("riposteFinale");
            FakeAddForceMotion(1);

            hasDamagedPlayer = true;
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasDamagedPlayer = false;
        animator.SetTrigger("riposteFinale");
    }

    private async void FakeAddForceMotion(float forceAmount)
    {
        float i = 0.01f;
        Rigidbody2D playerRb = player.gameObject.GetComponent<Rigidbody2D>();
        while (forceAmount > i)
        {
            if (player.localScale.x < 0)
            {
                playerRb.velocity = new Vector2(forceAmount / i, playerRb.velocity.y); // !! For X axis positive force
            }
            else
            {
                playerRb.velocity = new Vector2(-forceAmount / i, playerRb.velocity.y); // !! For X axis positive force
            }
            
            i = i + Time.deltaTime;
            await Task.Delay(17);
        }
        playerRb.velocity = Vector2.zero;
        await Task.Delay(1000);
    }
}
