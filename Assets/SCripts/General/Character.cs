using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
{
    [Header("事件监听")]
    public VoidEventSO newGameEvent;

    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;

    [Header("受伤无敌")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;


    public UnityEvent<Character> OnHealthChange;

    public UnityEvent<Transform> OnTakeDamage;  //unity引擎中的事件，声明事件传入坐标，在unity中可以注册各种不同方法作为事件发生时候执行的方法 
    public UnityEvent OnDie;  //创建死亡的事件执行相关方法（ 死亡时会触发很多方法，所以整一个事件）
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
        if (invulnerable)  //无敌判断
            return;

        if(currentHealth - attacker.damage > 0) //判断当前血量是否够承受一次伤害
        {
            currentHealth -= attacker.damage;  //减血 并且触发无敌的触发器
            // 执行受伤 
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(attacker.transform); //事件触发固定写法，？用来判断事件内是否有执行的方法，事件内可以有许多不同的方法，在事件触发时候同时调用
        }
        else
        {
            currentHealth = 0;
            //触发死亡
            OnDie?.Invoke();
        }
        OnHealthChange?.Invoke(this);
    }

    private void TriggerInvulnerable()  //无敌，碰撞出发无敌，以后翻滚无敌能用到应该
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

    public void GetSaveData(Data data) //数据保存
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

    public void LoadData(Data data)//加载数据
    {
        if(data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            this.currentHealth = data.floatSaveData[GetDataID().ID + "health"];
            this.currentPower = data.floatSaveData[GetDataID().ID + "power"];
            transform.position = data.characterPosDict[GetDataID().ID].ToVector3();

            //通知更新UI
            OnHealthChange?.Invoke(this);
        }
    }
}
