using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("监听事件")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;


    public PlayerinputControl inputControl;
    private Rigidbody2D rb;

    private PhysicCheck physicCheck;//获取physicCheck组件，来获得里面的地面检查Check（）方法，来进行人物跳跃的判断控制
    private PlayerAnimation playerAnimation;
    private Character character;
    private CapsuleCollider2D coll;

    //private SpriteRenderer sr;
    public Vector2 inputDirection;

    [Header("Basic parameters")]
    public float speed;
    //Jump 
    public float jumpForce;
    public float wallJumpForce;
    public float hurtForce;
    public float slideDictance;
    public float slideSpeed;
    public int slidePowerCost;

    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;



    [Header("状态")]
    //受伤状态
    public bool isHurt;
    //存活状态
    public bool isDead;
    //攻击判断
    public bool isAttack;
    //蹬墙跳的判断
    public bool wallJump;
    //滑铲状态
    public bool isSlide;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //获取自身刚体组件，来进行移动
        character = GetComponent<Character>();
        physicCheck = GetComponent<PhysicCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        coll = GetComponent<CapsuleCollider2D>();
        inputControl = new PlayerinputControl();//实例化
        //sr = GetComponent<SpriteRenderer>();

        //获取Jump
        inputControl.Gameplay.Jump.started += Jump;  //语法  += 事件的注册，started 按键按下的一刻（事件）

        //攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;


        //滑铲
        inputControl.Gameplay.Slide.started += Slide;
        inputControl.Enable();

    }



    private void OnEnable()
    {
        
        sceneLoadEvent.loadRequestEvent += OnLoadEvent;
        afterSceneLoadEvent.OnEventRasied += OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRasied += OnloadDataEvent;
        backToMenuEvent.OnEventRasied += OnloadDataEvent;
    }


    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.loadRequestEvent -= OnLoadEvent;
        afterSceneLoadEvent.OnEventRasied -= OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRasied -= OnloadDataEvent;
        backToMenuEvent.OnEventRasied -= OnloadDataEvent;
    }



    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();

        CheckState();
    }

    private void FixedUpdate()            //和物理有关的函数执行
    {
        if(!isHurt&&!isAttack)
            Move();
    }

    //测试
    //private void OnTriggerStay2D(Collider2D other)  //碰撞触发器，碰撞敌人
    //{
    //    Debug.Log(other.name);
    //}

    //读取游戏进度
    private void OnloadDataEvent()
    {
        isDead = false;
    }

    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();    //加载场景人物不能控制
    }
    private void OnAfterSceneLoadEvent()
    {
        inputControl.Gameplay.Enable();//场景加载事件之后，重新控制
    }
    public void Move()
    {
        if(!wallJump)
        {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y); 
        }

       /* 
        * 利用Filp进行翻转
        * if(inputDirection.x<0)
            sr.flipX = true;
        if(inputDirection.x>0)
            sr.flipX = false;*/

       int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;   
        //人物翻转，利用transfrom组件
        transform.localScale = new Vector3(faceDir, 1, 1);
    }
    private void PlayerAttack(InputAction.CallbackContext obj)
    {

        if(!physicCheck.isGround) //跳跃不攻击
            return;
        playerAnimation.PlayerAttack();//播放攻击动画
        isAttack = true;

    }
    private void Jump(InputAction.CallbackContext obj) //按下跳跃键传递inputAction  的一个callback方法
    {
        //Debug.Log("Jump");
        if(physicCheck.isGround )
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            GetComponent<AudioDefination>()?.PlayAudioClip(); //跳跃播放音效

            //打断滑铲的携程
            isSlide = false;
            StopAllCoroutines();
        }
        else if(physicCheck.onWall )  //蹬墙跳
        {
            rb.AddForce(new Vector2(-inputDirection.x, 2.5f) * wallJumpForce, ForceMode2D.Impulse);
            wallJump = true;
        }
    }
    private void Slide(InputAction.CallbackContext obj)
    {
        if(!isSlide && physicCheck.isGround &&character.currentPower>=slidePowerCost) //空中不能执行滑铲
        {
            isSlide = true;
            var targetPos = new Vector3(transform.localPosition.x + slideDictance * transform.localScale.x, transform.position.y);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            StartCoroutine(TriggerSlide(targetPos));


            character.OnSlide(slidePowerCost);
        }
    }

    private IEnumerator TriggerSlide(Vector3 target)
    {
        do
        {
            yield return null;
            if(!physicCheck.isGround)
                break;

            //滑动过程中撞墙
            if(physicCheck.touchLeftWall && transform.localScale.x <0f||physicCheck.touchRightWall && transform.localScale.x > 0f)
            {
                isSlide=false;
                break;
            }

            rb.MovePosition(new Vector2(transform.position.x + transform.localScale.x * slideSpeed, transform.position.y));
        } while (MathF.Abs(target.x - transform.position.x) > 0.1f);
        isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    #region UnityEvent 中执行
    public void GetHurt(Transform attacker) //人物受伤反弹的方法
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.transform.position.x),0).normalized;

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }
    public void PlayerDead()
    {
        isDead  = true;
        inputControl.Gameplay.Disable();
    }
    #endregion

    private void CheckState()
    {
        coll.sharedMaterial = physicCheck.isGround ? normal : wall;
        if (physicCheck.onWall)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
        else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);


        if(wallJump&&rb.velocity.y<0f)
        {
            wallJump = false;
        }
        //避免鞭尸
        if (isDead && isSlide)
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        else
            gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
