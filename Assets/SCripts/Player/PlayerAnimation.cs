using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim; //ͬ���Ļ�ȡ�ű��������е������������animator �����ڿ�ʼʱ�����һ�� awake��
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
    public void SetAnimation()//���еĶ����л����룬��Ҫÿ֡���ã����Է���Update()�� 
    {
        anim.SetFloat("velocityX",Mathf.Abs(rb.velocity.x)); //ȡ����ֵ�ж��ٶ��Ƿ���ڣ�Ȼ���л��ܲ�����
        //��Ծ����
        anim.SetFloat("velocityY", rb.velocity.y);
        //��ض���
        anim.SetBool("isGround", physicCheck.isGround);
        //��������
        anim.SetBool("isDead",playerController.isDead);
        // �Ƿ񹥻�
        anim.SetBool("isAttack", playerController.isAttack);
        //��ǹ����
        anim.SetBool("onWall", physicCheck.onWall);
        //��������
        anim.SetBool("isSlide", playerController.isSlide);
    }

    public void PlayerHurt() //trigger���ͣ�����Ҫÿ֡���� ����дһ������  ��������ʱ��ִ��
    {
        anim.SetTrigger("hurt");
    }
    public void PlayerAttack()
    {
        anim.SetTrigger("attack");
    }
}
