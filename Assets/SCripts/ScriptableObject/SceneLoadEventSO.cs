using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/SceneLoadEventSO")]

public class SceneLoadEventSO :ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> loadRequestEvent;


    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="loactionToLoad">Ҫ���صĳ���</param>
    /// <param name="posToGO">Ŀ������</param>
    /// <param name="fadeScreen">�Ƿ��뵭��</param>
    public void RaiseLoadRequestEvent(GameSceneSO loactionToLoad,Vector3 posToGO,bool fadeScreen)
    {
        loadRequestEvent?.Invoke(loactionToLoad, posToGO, fadeScreen);
    }
}
