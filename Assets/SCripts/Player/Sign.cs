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
    private Animator anim;  //anim������������������
    public Transform playerTrans;
    // ����gameobject���ͣ����ƿ���
    public GameObject signSprite;

    private IInteractable targetItem;

    public bool canPress;

    private void Awake()
    {
        //anim = GetComponentInChildren<Animator>();   //���animator ����1����Ϊһ��ʼ����رգ�û�취��ȡ�����÷���2
        anim = signSprite.GetComponent<Animator>(); //����2  

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
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress; //����ʱ������
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
    /// �л��豸ͬʱ�л�����
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
                    //Debug.Log("�л��ɹ�");
                    anim.Play("Xbox");
                    break;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)  //ͨ����ǩ�ж�
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
