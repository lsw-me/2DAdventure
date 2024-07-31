using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour,ISaveable
{
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector3 menuPosition;
    [Header("�¼�����")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;  //��������Ϸ���¼�
    public VoidEventSO backToMenuEvent;

    //public GameSceneSO secondLoadScene;
    [Header("�㲥")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO unloadedSceneEvent;

    [Header("����")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
    private GameSceneSO sceneToLoad;
    private GameSceneSO currentLoadScene;
    private Vector3 positionToGo;
    private bool fadeScreen;
    private bool isLoading;

    public float fadeDuration;
    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference,LoadSceneMode.Additive);
        //currentLoadScene = firstLoadScene;
        //currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
       
    }

    //TODO:�������˵���Ҫ��
    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        //NewGame();
    }
    private void OnEnable()
    {
        loadEventSO.loadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRasied += NewGame;//�¼���ע��
        backToMenuEvent.OnEventRasied += OnBackToMenuEvent;


        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable()
    {
        loadEventSO.loadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRasied -= NewGame;
        backToMenuEvent.OnEventRasied -= OnBackToMenuEvent;

        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void OnBackToMenuEvent()
    {
        sceneToLoad = menuScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, menuPosition, true);
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        //OnLoadRequestEvent(sceneToLoad, firstPosition, true);
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);

    }


    /// <summary>
    /// ���������¼�����
    /// </summary>
    /// <param name="locationToGo"></param>
    /// <param name="posToGo"></param>
    /// <param name="fadeScreen"></param>


    private void OnLoadRequestEvent(GameSceneSO locationToGo, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
            return;
        isLoading = true;
        sceneToLoad = locationToGo;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen; //�������
        if (currentLoadScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }

    private IEnumerator UnLoadPreviousScene()
    {
        if(fadeScreen)
        {
            //TODO: ʵ�ֽ��뽥��
            fadeEvent.FadeIn(fadeDuration); 
        }
        yield return new WaitForSeconds(fadeDuration);
        
        //�㲥�¼�����Ѫ����ʾ
        unloadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad,positionToGo,true);


        yield return currentLoadScene.sceneReference.UnLoadScene();
        //�ر�����
        playerTrans.gameObject.SetActive(false);
        //�����³���
        LoadNewScene();
    }
    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive,true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle) //��������֮��ִ�еķ���
    {
        currentLoadScene = sceneToLoad;
        playerTrans.position = positionToGo;

        playerTrans.gameObject.SetActive(true);
        if(fadeScreen)
        {
            //TODO:
            fadeEvent.FadeOut(fadeDuration);
        }
        isLoading = false;

        if(currentLoadScene.SceneType ==SceneType.Loaction)
        {
            //����������ɺ��¼�
            //Debug.Log(currentLoadScene.SceneType);
            afterSceneLoadedEvent.RasieEvent();
        }
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefination>().ID;
        if(data.characterPosDict.ContainsKey(playerID))  //�жϱ���������û��
        {
            positionToGo = data.characterPosDict[playerID].ToVector3();
            sceneToLoad = data.GetSaveScene();

            OnLoadRequestEvent(sceneToLoad, positionToGo, true);
        }
    }
}
