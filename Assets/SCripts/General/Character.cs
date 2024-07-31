using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
{
    [Header("�¼�����")]
    public VoidEventSO newGameEvent;

    [Header("��������")]
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;

    [Header("�����޵�")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;


    public UnityEvent<Character> OnHealthChange;

    public UnityEvent<Transform> OnTakeDamage;  //unity�����е��¼��������¼��������꣬��unity�п���ע����ֲ�ͬ������Ϊ�¼�����ʱ��ִ�еķ��� 
    public UnityEvent OnDie;  //�����������¼�ִ����ط����� ����ʱ�ᴥ���ܶ෽����������һ���¼���
    private void NewGame()
    {
        currentHealth = maxHealth;
        currentPower = maxPower;
        OnHealthChange?.Invoke(this);
    }
    private void OnEnable()
    {
        newGameEvent.OnEventRasied += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable()
    {
        newGameEvent.OnEventRasied -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }
    private void Update()
    {
        if(invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if(invulnerableCounter <= 0)
            {
                invulnerable = false;

            }
        }

        if(currentPower <maxPower)
        {
            currentPower += Time.deltaTime * powerRecoverSpeed;
        }
            
    }


    public void TakeDamage(Attack attacker)
    {
        if (invulnerable)  //�޵��ж�
            return;

        if(currentHealth - attacker.damage > 0) //�жϵ�ǰѪ���Ƿ񹻳���һ���˺�
        {
            currentHealth -= attacker.damage;  //��Ѫ ���Ҵ����޵еĴ�����
            // ִ������ 
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(attacker.transform); //�¼������̶�д�����������ж��¼����Ƿ���ִ�еķ������¼��ڿ�������಻ͬ�ķ��������¼�����ʱ��ͬʱ����
        }
        else
        {
            currentHealth = 0;
            //��������
            OnDie?.Invoke();
        }
        OnHealthChange?.Invoke(this);
    }

    private void TriggerInvulnerable()  //�޵У���ײ�����޵У��Ժ󷭹��޵����õ�Ӧ��
    {
       if(!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }    
    }

    public void  OnSlide(int cost)
    {
        currentPower -=cost;
        OnHealthChange?.Invoke(this);
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data) //���ݱ���
    {
        if(data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = new SerializeVector3(transform.position);
            data.floatSaveData[GetDataID().ID + "health"] = this.currentHealth;
            data.floatSaveData[GetDataID().ID + "power"] = this.currentPower;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID , new SerializeVector3(transform.position));
            data.floatSaveData.Add(GetDataID().ID+ "health",this.currentHealth);
            data.floatSaveData.Add(GetDataID().ID + "power", this.currentPower);
        }
    }

    public void LoadData(Data data)//��������
    {
        if(data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            this.currentHealth = data.floatSaveData[GetDataID().ID + "health"];
            this.currentPower = data.floatSaveData[GetDataID().ID + "power"];
            transform.position = data.characterPosDict[GetDataID().ID].ToVector3();

            //֪ͨ����UI
            OnHealthChange?.Invoke(this);
        }
    }
}
