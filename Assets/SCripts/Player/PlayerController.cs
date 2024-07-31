using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("�����¼�")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;


    public PlayerinputControl inputControl;
    private Rigidbody2D rb;

    private PhysicCheck physicCheck;//��ȡphysicCheck��������������ĵ�����Check����������������������Ծ���жϿ���
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

    [Header("�������")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;



    [Header("״̬")]
    //����״̬
    public bool isHurt;
    //���״̬
    public bool isDead;
    //�����ж�
    public bool isAttack;
    //��ǽ�����ж�
    public bool wallJump;
    //����״̬
    public bool isSlide;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //��ȡ�������������������ƶ�
        character = GetComponent<Character>();
        physicCheck = GetComponent<PhysicCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        coll = GetComponent<CapsuleCollider2D>();
        inputControl = new PlayerinputControl();//ʵ����
        //sr = GetComponent<SpriteRenderer>();

        //��ȡJump
        inputControl.Gameplay.Jump.started += Jump;  //�﷨  += �¼���ע�ᣬstarted �������µ�һ�̣��¼���

        //����
        inputControl.Gameplay.Attack.started += PlayerAttack;


        //����
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

    private void FixedUpdate()            //�������йصĺ���ִ��
    {
        if(!isHurt&&!isAttack)
            Move();
    }

    //����
    //private void OnTriggerStay2D(Collider2D other)  //��ײ����������ײ����
    //{
    //    Debug.Log(other.name);
    //}

    //��ȡ��Ϸ����
    private void OnloadDataEvent()
    {
        isDead = false;
    }

    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();    //���س������ﲻ�ܿ���
    }
    private void OnAfterSceneLoadEvent()
    {
        inputControl.Gameplay.Enable();//���������¼�֮�����¿���
    }
    public void Move()
    {
        if(!wallJump)
        {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y); 
        }

       /* 
        * ����Filp���з�ת
        * if(inputDirection.x<0)
            sr.flipX = true;
        if(inputDirection.x>0)
            sr.flipX = false;*/

       int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;   
        //���﷭ת������transfrom���
        transform.localScale = new Vector3(faceDir, 1, 1);
    }
    private void PlayerAttack(InputAction.CallbackContext obj)
    {

        if(!physicCheck.isGround) //��Ծ������
            return;
        playerAnimation.PlayerAttack();//���Ź�������
        isAttack = true;

    }
    private void Jump(InputAction.CallbackContext obj) //������Ծ������inputAction  ��һ��callback����
    {
        //Debug.Log("Jump");
        if(physicCheck.isGround )
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            GetComponent<AudioDefination>()?.PlayAudioClip(); //��Ծ������Ч

            //��ϻ�����Я��
            isSlide = false;
            StopAllCoroutines();
        }
        else if(physicCheck.onWall )  //��ǽ��
        {
            rb.AddForce(new Vector2(-inputDirection.x, 2.5f) * wallJumpForce, ForceMode2D.Impulse);
            wallJump = true;
        }
    }
    private void Slide(InputAction.CallbackContext obj)
    {
        if(!isSlide && physicCheck.isGround &&character.currentPower>=slidePowerCost) //���в���ִ�л���
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

            //����������ײǽ
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

    #region UnityEvent ��ִ��
    public void GetHurt(Transform attacker) //�������˷����ķ���
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
        //�����ʬ
        if (isDead && isSlide)
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        else
            gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
