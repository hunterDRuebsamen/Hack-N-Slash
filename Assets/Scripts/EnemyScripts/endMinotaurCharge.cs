using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endMinotaurCharge : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    minotaurBehavior mb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mb = animator.GetComponent<minotaurBehavior>();
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.ResetTrigger("charge");
       animator.ResetTrigger("chargeFinale");
       mb.chargeTimer(10000);
       //mb.canAttack = true;
    }
}
