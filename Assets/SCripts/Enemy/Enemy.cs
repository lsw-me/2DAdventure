using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    protected Rigidbody2D rb;
    [HideInInspector]public Animator anim;   //不写修饰符默认私有

    [HideInInspector] public PhysicCheck physicsCheck;

    [Header("基本参数")]
    public float normalSpeed; //平时速度
    public float chaseSpeed; //追人速度
    public float currentSpeed; //当前速度
    public Vector3 faceDir;//面朝方向
    public float hurtForce;//受伤时候击退的力

    public Transform attacker;  //用于被攻击时候，记录攻击者，然后攻击攻击者

    [Header("检测")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    [Header("计时器")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;
    public float lostTime;
    public float lostTimeCounter;

    [Header("状态")]
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
        faceDir = new Vector3(-transform.localScale.x, 0, 0);  //实时获得敌人面朝方向，问题（一直在new vector3 是否对性能有影响）

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
    public virtual void Move() //移动  虚方法，希望子类可以修改父类
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x*Time.deltaTime,rb.velocity.y);  
    }

    public void TimeCounter() //计时器
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

    #region 事件执行方法
    public void OnTakeDamage(Transform attackTrans) //受伤，伤害来源玩家
    {
        attacker = attackTrans; //记录攻击者
        // 转身
        if(attackTrans.position.x - transform.position.x >0) //玩家在右侧
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if(attacker.position.x - transform.position.x <0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        //受伤击退
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
