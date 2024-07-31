using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using System.IO;


[DefaultExecutionOrder(-100)]

public class DataManager : MonoBehaviour
{
    public static DataManager instance;  //����ģʽ ��̬
    [Header("�¼�����")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;

    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;

    private string jsonFolder; //�洢·��
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        saveData = new Data();

        jsonFolder = Application.persistentDataPath + "/SAVE DATA/"; //�����κ�ƽ̨����Ĭ��λ�� windows��user/Appdata /.../...��

        ReadSavedData();
    }
    private void OnEnable()
    {
        saveDataEvent.OnEventRasied += Save;
        loadDataEvent.OnEventRasied += Load;
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRasied -= Save;
        loadDataEvent.OnEventRasied -= Load;
    }

    private void Update()
    {
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }

    public void RegisterSaveData(ISaveable saveable)
    {
        if(!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    public void Save()
    {
        foreach (var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }
        var resultPath = jsonFolder + "data.sav";

        var jsonData = JsonConvert.SerializeObject(saveData);
        if(!File.Exists(resultPath))                                 //����ļ�û�д���
        {
            Directory.CreateDirectory(jsonFolder);
        }

        File.WriteAllText(resultPath, jsonData); //д�ļ�
       
        // foreach (var item in saveData.characterPosDict)
       // {
       //    Debug.Log(item.Key + " " + item.Value);
       // }
    }
    public void Load() 
    {
        foreach (var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }

    private void ReadSavedData()
    {
        var resultPath = jsonFolder + "data.sav";
        if (File.Exists(resultPath))                                 //����ļ�û�д���
        {
            var stringData = File.ReadAllText(resultPath);
            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);
            saveData = jsonData;
        }   
    }
}
