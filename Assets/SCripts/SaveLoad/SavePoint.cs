using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour,IInteractable
{
    [Header("广播")]
    public VoidEventSO SaveGameEvent;


    [Header("变量参数")]
    public SpriteRenderer spriteRenderer;  //在unity中进行实体化
    public GameObject lightObj;
    public Sprite blackSprite;
    public Sprite lightSprite;
    public bool isSave;


    private void OnEnable()
    {
        spriteRenderer.sprite = isSave ? lightSprite : blackSprite;
        lightObj.SetActive(isSave);
    }

    public void TriggerAction()
    {
        if(!isSave)
        {
            isSave = true;
            spriteRenderer.sprite = lightSprite;

            lightObj.SetActive(true);
            //TODO:保存数据

            SaveGameEvent.RasieEvent(); 
            this.gameObject.tag = "Untagged";
        }
    }
}
