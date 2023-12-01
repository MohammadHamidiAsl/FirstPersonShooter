using System.Collections.Generic;
using UnityEngine;
using GameSystem.Core;
using UnityEngine.Serialization;

public enum SoundEffect
{
    Footsteps,
    Reload,
    Fire,
    Empty
}


public class AudioManagementService : MonoBehaviour, IAudioManagementService
{
    public AudioData AudioData;
    public AudioSource LoopSource;
    public AudioSource OnceSource;

    private Dictionary<SoundEffect, AudioClip> soundEffects;


    public void Initialize(AudioData audioData)
    {
        AudioData = audioData;
        soundEffects = new Dictionary<SoundEffect, AudioClip>();

        soundEffects.Add(SoundEffect.Footsteps, AudioData.FootSteps);
        soundEffects.Add(SoundEffect.Reload, AudioData.Reload);
        soundEffects.Add(SoundEffect.Fire, AudioData.Fire);
        soundEffects.Add(SoundEffect.Empty, AudioData.Empty);
    }

    public void PlaySound(SoundEffect effect, bool loop = false)
    {
        if (loop)
        {
            if (!LoopSource.isPlaying)
            {
                LoopSource.clip = soundEffects[effect];
                LoopSource.loop = true;
                LoopSource.Play();
            }
        }
        else
        {
            OnceSource.PlayOneShot(soundEffects[effect]);
        }
    }

    public void StopLoopingSound()
    {
        if (LoopSource.isPlaying)
        {
            LoopSource.Stop();
        }
    }
}