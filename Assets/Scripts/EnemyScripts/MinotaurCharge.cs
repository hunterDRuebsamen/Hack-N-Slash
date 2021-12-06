using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurCharge : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    minotaurBehavior mb;
    BoxCollider2D hitbox;
    bool hasDamagedPlayer = false;
    Vector2 previousPlayerPos;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindWithTag("Player").transform;
       rb = animator.GetComponent<Rigidbody2D>();
       hitbox = animator.GetComponentInChildren<BoxCollider2D>();
       mb = animator.GetComponent<minotaurBehavior>();
       previousPlayerPos = player.position;
       animator.ResetTrigger("chargeFinale");
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, previousPlayerPos, mb.speed*Time.deltaTime*5);
        rb.MovePosition(newPos);
        if (hitbox.IsTouching(player.GetComponent<CapsuleCollider2D>()) && !hasDamagedPlayer) {
            // the hitbox is touching the player capsule collider, deal damage!
            mb.Attack();
            animator.SetTrigger("chargeFinale");
            FakeAddForceMotion(1);

            hasDamagedPlayer = true;
        }
        if (rb.position == previousPlayerPos) {
            animator.SetTrigger("chargeFinale");
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasDamagedPlayer = false;
        animator.SetTrigger("chargeFinale");
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
