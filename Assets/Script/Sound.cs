using UnityEngine;
using System;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    public bool isPlayOnAwake;
    public bool loop;

    [Range(0f, 1f)]
    public float volume = 1;

    //[HideInInspector]
    public AudioSource audioSource;
}
