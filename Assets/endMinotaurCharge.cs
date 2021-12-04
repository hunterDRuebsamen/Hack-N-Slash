using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endMinotaurCharge : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    minotaurBehavior mb;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.ResetTrigger("charge");
       mb.chargeTimer(5000);
    }
}
