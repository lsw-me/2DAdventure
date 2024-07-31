using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public PlayerStatBar playerStatBar;
    [Header("�¼�����")]
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO unloadedSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public FloatEventSO syncVolumeEvent;
    [Header("�¼��Ĺ㲥")]
    public VoidEventSO pauseEvent;

    [Header("���")]
    public GameObject gameOverPanel;
    public GameObject restartBtn;
    public GameObject mobileTouch;//������Ļ����
    public Button settingByn;
    public GameObject pausePanel;
    public Slider volumeSlider;


    private void Awake()
    { 
#if UNITY_STANDALONE
        mobileTouch.SetActive(false);
#endif


        settingByn.onClick.AddListener(TogglePausePanel);
    }
    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        unloadedSceneEvent.loadRequestEvent += OnUnLoadedSceneEvent;
        loadDataEvent.OnEventRasied += OnLoadDataEvent;
        gameOverEvent.OnEventRasied += OnGameOverEvent;
        backToMenuEvent.OnEventRasied += OnLoadDataEvent;
        syncVolumeEvent.OnEventRasied += OnSyncVolumeEvent;
    }


    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        unloadedSceneEvent.loadRequestEvent -= OnUnLoadedSceneEvent;
        loadDataEvent.OnEventRasied -= OnLoadDataEvent;
        gameOverEvent.OnEventRasied -= OnGameOverEvent;
        backToMenuEvent.OnEventRasied -= OnLoadDataEvent;
        syncVolumeEvent.OnEventRasied -= OnSyncVolumeEvent;
    }



    private void TogglePausePanel()
    {
        if(pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pauseEvent.RasieEvent();
            pausePanel.SetActive(true); 
            Time.timeScale = 0;
        }
    }

    private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount+80)/100;
    }
    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn);
    }

    private void OnLoadDataEvent()
    {
        gameOverPanel.SetActive(false); //�ص��������
    }

    private void OnUnLoadedSceneEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {

        var isMenu = sceneToLoad.SceneType == SceneType.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
        
        //�ϱ��Ǽ�д�����±߸��Ӱ汾
        //if (sceneToLoad.SceneType ==SceneType.Menu)
        //{
        //    playerStatBar.gameObject.SetActive(false);
        //}
        //if(sceneToLoad.SceneType == SceneType.Loaction)
        //{
        //    playerStatBar.gameObject.SetActive(true);
        //}
    }

    private void OnHealthEvent(Character character)
    {
        var persentage = character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(persentage);

        playerStatBar.OnPowerChange(character);
    }
} 
