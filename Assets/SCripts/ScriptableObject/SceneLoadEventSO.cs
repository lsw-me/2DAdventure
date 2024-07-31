using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/SceneLoadEventSO")]

public class SceneLoadEventSO :ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> loadRequestEvent;


    /// <summary>
    /// 场景加载请求
    /// </summary>
    /// <param name="loactionToLoad">要加载的场景</param>
    /// <param name="posToGO">目的坐标</param>
    /// <param name="fadeScreen">是否淡入淡出</param>
    public void RaiseLoadRequestEvent(GameSceneSO loactionToLoad,Vector3 posToGO,bool fadeScreen)
    {
        loadRequestEvent?.Invoke(loactionToLoad, posToGO, fadeScreen);
    }
}
