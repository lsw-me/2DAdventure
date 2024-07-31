using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim; //同样的获取脚本所挂载中的组件（这里是animator 并且在开始时候调用一次 awake）
    private Rigidbody2D rb;
    private PhysicCheck physicCheck;
    private PlayerController playerController;

    private void Awake()
    {
        anim = GetComponent<Animator>();   
        rb = GetComponent<Rigidbody2D>();
        physicCheck = GetComponent<PhysicCheck>();
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        SetAnimation();
    }
    public void SetAnimation()//所有的动画切换代码，需要每帧调用，所以放在Update()里 
    {
        anim.SetFloat("velocityX",Mathf.Abs(rb.velocity.x)); //取绝对值判断速度是否大于，然后切换跑步动画
        //跳跃动画
        anim.SetFloat("velocityY", rb.velocity.y);
        //落地动画
        anim.SetBool("isGround", physicCheck.isGround);
        //死亡动画
        anim.SetBool("isDead",playerController.isDead);
        // 是否攻击
        anim.SetBool("isAttack", playerController.isAttack);
        //滑枪动画
        anim.SetBool("onWall", physicCheck.onWall);
        //滑铲动画
        anim.SetBool("isSlide", playerController.isSlide);
    }

    public void PlayerHurt() //trigger类型，不需要每帧触发 单独写一个方法  人物受伤时候执行
    {
        anim.SetTrigger("hurt");
    }
    public void PlayerAttack()
    {
        anim.SetTrigger("attack");
    }
}
