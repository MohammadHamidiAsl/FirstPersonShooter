using UnityEngine;

[CreateAssetMenu(menuName = "Data/Audio/AudioClips",fileName = "AudioData")]
public class AudioData:ScriptableObject
{
    public AudioClip FootSteps;
    public AudioClip Reload;
    public AudioClip Fire;
    public AudioClip Empty;

}