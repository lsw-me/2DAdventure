using TMPro;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Event/FloatEventSO")]
public class FloatEventSO : ScriptableObject
{
    public UnityAction<float> OnEventRasied;

    public void RasieEvent(float amount)
    {
        OnEventRasied?.Invoke(amount);
    }
}