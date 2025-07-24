using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class BGMusicManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioMixerGroup musicGroup;
    
    void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;

            sound.source.outputAudioMixerGroup = musicGroup;
            sound.source.loop = true;
            sound.source.playOnAwake = true;
        }

    }

    void Start()
    {
        Play("Music");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("No music");
            return;
        }
        s.source.Play();
    }
}
