using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class Sign : MonoBehaviour
{
    private PlayerinputControl playerInput;
    private Animator anim;  //anim挂载在他的子物体上
    public Transform playerTrans;
    // 创建gameobject类型，控制开关
    public GameObject signSprite;

    private IInteractable targetItem;

    public bool canPress;

    private void Awake()
    {
        //anim = GetComponentInChildren<Animator>();   //获得animator 方法1，因为一开始组件关闭，没办法获取所以用方法2
        anim = signSprite.GetComponent<Animator>(); //方法2  

        playerInput = new PlayerinputControl();
        playerInput.Enable();
    }
    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        playerInput.Gameplay.Confirm.started += OnConfirm;
    }

    private void OnDisable()
    {
        canPress = false; 
    }
    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress; //控制时候启动
        signSprite.transform.localScale = playerTrans.localScale;
    }


    private void OnConfirm(InputAction.CallbackContext context)
    {
        if(canPress)
        {
            targetItem.TriggerAction();
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }


    /// <summary>
    /// 切换设备同时切换动画
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="actionChange"></param>
    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if(actionChange ==InputActionChange.ActionStarted)
        {
            //Debug.Log(((InputAction)obj).activeControl.device);

            var d = ((InputAction)obj).activeControl.device;

            switch(d.device)
            {
                case Keyboard :
                    anim.Play("KeyBoard");
                    break;
                case InputDevice:
                    //Debug.Log("切换成功");
                    anim.Play("Xbox");
                    break;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)  //通过标签判断
    {
        if(collision.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = collision.GetComponent<IInteractable>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
    }

}
