using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO sceneToGo;
    public Vector3 positionToGO;
    public void TriggerAction()
    {
        Debug.Log("����");
        loadEventSO.RaiseLoadRequestEvent(sceneToGo,positionToGO,true);
    }
}
