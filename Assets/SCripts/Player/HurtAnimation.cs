using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtAnimation : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state  动画进入
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks  动画持续不断执行中
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state  动画退出的时候
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)   //这里进行修改当，动画退出的时候，我们吧挂载也就是传入的（animator）中的值更改
    {
        animator.GetComponent<PlayerController>().isHurt = false;  //获取playerControl组件然后修改
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion 
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)d'p'm'n
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
