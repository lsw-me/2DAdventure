using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName ="Event/PlayAudioEventSO")]
public class PlayAudioEventSO : ScriptableObject
{
    public UnityAction<AudioClip> OnEventRaised;


    public void RasieEvent(AudioClip audioClip)
    {
        OnEventRaised?.Invoke(audioClip);
    }

}
