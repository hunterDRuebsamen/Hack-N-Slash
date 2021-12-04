using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blocking : StateMachineBehaviour
{
    EnemyBase enemyBase;
    WeaponAbilities weaponAbility;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       enemyBase = animator.GetComponent<EnemyBase>();
       if(weaponAbility.breakBlock == false)
         enemyBase.isBlocking = true;
       Debug.Log("Enemy is blocking");
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       enemyBase.isBlocking = false;
    }


}
