using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    [Header("�¼�����")]
    public VoidEventSO afterSceneLoadedEvent;
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;
    public VoidEventSO cameraShakeEvent;
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D> ();
    }

    private void OnEnable()
    {
        cameraShakeEvent.OnEventRasied += OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRasied += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRasied -= OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRasied -= OnAfterSceneLoadedEvent;
    }
    private void OnAfterSceneLoadedEvent()
    {
        GetNewCameraBounds();
    }

    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }


    //TO

    //private void Start()
    //{
    //    GetNewCameraBounds ();
    //}

    private void  GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
            return;
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>(); //Collider2D ����������ײ��

        confiner2D.InvalidateCache();    //�������
    }
}
