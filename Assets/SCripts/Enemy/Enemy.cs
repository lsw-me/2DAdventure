using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    protected Rigidbody2D rb;
    [HideInInspector]public Animator anim;   //��д���η�Ĭ��˽��

    [HideInInspector] public PhysicCheck physicsCheck;

    [Header("��������")]
    public float normalSpeed; //ƽʱ�ٶ�
    public float chaseSpeed; //׷���ٶ�
    public float currentSpeed; //��ǰ�ٶ�
    public Vector3 faceDir;//�泯����
    public float hurtForce;//����ʱ����˵���

    public Transform attacker;  //���ڱ�����ʱ�򣬼�¼�����ߣ�Ȼ�󹥻�������

    [Header("���")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    [Header("��ʱ��")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;
    public float lostTime;
    public float lostTimeCounter;

    [Header("״̬")]
    public bool isHurt;
    public bool isDead;

    protected BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicCheck>();
        currentSpeed =  normalSpeed;
    }
    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);  //ʵʱ��õ����泯�������⣨һֱ��new vector3 �Ƿ��������Ӱ�죩

        currentState.LogicUpdate(); 
        TimeCounter();
    }

    private void FixedUpdate()
    {
        if(!isHurt&&!isDead && !wait)
        {
            Move();
        }
        currentState.PhysicsUpdate();
    }
    private void OnDisable()
    {
        currentState.OnExit();  
    }
    public virtual void Move() //�ƶ�  �鷽����ϣ����������޸ĸ���
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x*Time.deltaTime,rb.velocity.y);  
    }

    public void TimeCounter() //��ʱ��
    {
        if(wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if(waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
        if(!FindPlayer()&&lostTimeCounter>0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
        else if(FindPlayer())
        {
            lostTimeCounter = lostTime;
        }
    }
    public bool FindPlayer()
    {
        return Physics2D.BoxCast((Vector2)transform.position + centerOffset,checkSize,0,faceDir,checkDistance,attackLayer); 

    }
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null

        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    #region �¼�ִ�з���
    public void OnTakeDamage(Transform attackTrans) //���ˣ��˺���Դ���
    {
        attacker = attackTrans; //��¼������
        // ת��
        if(attackTrans.position.x - transform.position.x >0) //������Ҳ�
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if(attacker.position.x - transform.position.x <0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        //���˻���
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x -attacker.position.x,0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(dir));
        
    }
    IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        isHurt = false; 
    }

    public void Ondie()
    {
        gameObject.layer = 2;
        anim.SetBool("dead", true);
        isDead = true;
    }

    public void DestoryAfterAnimation()
    {
        Destroy(this.gameObject);
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f); 
    } 
}
