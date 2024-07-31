using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/CharacterEventSo")]
public class CharacterEventSO : ScriptableObject //不能挂载当组件了
{
    public UnityAction<Character> OnEventRaised; 

    public void RaiseEvent(Character character)
    {
        OnEventRaised?.Invoke(character);
    }
}
