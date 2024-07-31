using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicCheck : MonoBehaviour
{

    private CapsuleCollider2D coll;
    private PlayerController playerController;
    private Rigidbody2D rb; //�����ж���Ծ��
    [Header("������")]
    public bool manual;  //�ֶ����ü��
    public bool isPlayer;
    public Vector2 bottomOffect; //�ŵ�λ�Ʋ�ֵ������
    public Vector2 leftOffset;
    public Vector2 rightOffset; // ���������ƫ������
    public float checkRaduis; //����public����������unity��ֱ�ӱ༭
    public LayerMask groundLayer; //Layermasak ����������ײ��layer

    [Header("״̬")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    public bool onWall;
    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        if(!manual) //��������ֶ����ã���ʼ����һ���̶�ֵ
        {
            rightOffset = new Vector2((coll.bounds.size.x)/2 + coll.offset.x,(coll.bounds.size.y) / 2 );
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
        if(isPlayer)
            playerController = GetComponent<PlayerController>(); //����ҲŻ�����
    }
    private void Update() 
    {
        Check();
    }
    
    //�������ϵļ����ײ
    public  void Check()
    {
        //�����棬�ڵ����ϣ�������Ծ�����ڽ�ֹ
        if (onWall)
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffect.x * transform.localScale.x, bottomOffect.y), checkRaduis, groundLayer);
        else
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffect.x * transform.localScale.x, 0), checkRaduis, groundLayer);

        //ǽ���ж�
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position +leftOffset,checkRaduis,groundLayer); 
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);

        //��ǽ��
        if(isPlayer)
        {
            onWall = (touchLeftWall && playerController.inputDirection.x < 0f || touchRightWall && playerController.inputDirection.x > 0f) && rb.velocity.y<0f;

        }
    }

    private void OnDrawGizmosSelected() //unity �Դ��Ļ��Ʒ���//���㿴�����ǽŵ׼��ķ�Χ��0.2�� ѡ������ʱ�Զ�ִ��
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffect,checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}
