using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    
    public Sound[] sounds;
    private void Awake()
    {
        if (sounds == null) return;
        foreach (Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();

            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
            sound.audioSource.name = sound.name;
            sound.audioSource.playOnAwake = sound.isPlayOnAwake;
        }
    }

    private void Start()
    {
        if (sounds == null) return;
        PlayAudio("BGM");
    }

    public void PlayAudio(string name)
    {
        //Sound sound = Array.Find(sounds, x => x.name == name);
        //sound.audioSource.Play();

        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                sound.audioSource.Play();
                Debug.Log(name + "is playing");
            }
        }
    }

    public void PlayWholeAudio(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                if (sound.audioSource.isPlaying) return;
                sound.audioSource.Play();
            }
        }
    }
}
