using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicCheck : MonoBehaviour
{

    private CapsuleCollider2D coll;
    private PlayerController playerController;
    private Rigidbody2D rb; //用来判断跳跃最
    [Header("检测参数")]
    public bool manual;  //手动设置检测
    public bool isPlayer;
    public Vector2 bottomOffect; //脚底位移差值的修正
    public Vector2 leftOffset;
    public Vector2 rightOffset; // 左右两侧的偏差修正
    public float checkRaduis; //创建public变量方便在unity里直接编辑
    public LayerMask groundLayer; //Layermasak 变量表面碰撞的layer

    [Header("状态")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    public bool onWall;
    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        if(!manual) //如果不是手动设置，开始等于一个固定值
        {
            rightOffset = new Vector2((coll.bounds.size.x)/2 + coll.offset.x,(coll.bounds.size.y) / 2 );
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
        if(isPlayer)
            playerController = GetComponent<PlayerController>(); //是玩家才获得组件
    }
    private void Update() 
    {
        Check();
    }
    
    //持续不断的检查碰撞
    public  void Check()
    {
        //检测地面，在地面上，允许跳跃，不在禁止
        if (onWall)
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffect.x * transform.localScale.x, bottomOffect.y), checkRaduis, groundLayer);
        else
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffect.x * transform.localScale.x, 0), checkRaduis, groundLayer);

        //墙体判断
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position +leftOffset,checkRaduis,groundLayer); 
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);

        //在墙上
        if(isPlayer)
        {
            onWall = (touchLeftWall && playerController.inputDirection.x < 0f || touchRightWall && playerController.inputDirection.x > 0f) && rb.velocity.y<0f;

        }
    }

    private void OnDrawGizmosSelected() //unity 自带的绘制方法//方便看到我们脚底检测的范围（0.2） 选中物体时自动执行
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffect,checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}
