using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour,IInteractable
{
    [Header("�㲥")]
    public VoidEventSO SaveGameEvent;


    [Header("��������")]
    public SpriteRenderer spriteRenderer;  //��unity�н���ʵ�廯
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
            //TODO:��������

            SaveGameEvent.RasieEvent(); 
            this.gameObject.tag = "Untagged";
        }
    }
}
