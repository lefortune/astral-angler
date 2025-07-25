using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] sounds;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup foleyGroup;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;

            if (sound.name == "Music")
            {
                sound.source.outputAudioMixerGroup = musicGroup;
                sound.source.loop = true;
                sound.source.playOnAwake = true;
            }
            else if (sound.group == "SFX")
            {
                sound.source.outputAudioMixerGroup = sfxGroup;
            }
            else
            {
                sound.source.outputAudioMixerGroup = foleyGroup;
            }

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

    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found: " + name);
            return;
        }
        s.source.PlayOneShot(s.clip); // Use PlayOneShot for sound effects (won't interrupt)
    }

    public void PlayVaried(string name, float minPitch = 0.6f, float maxPitch = 1.4f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }
        float originalPitch = s.source.pitch;
        s.source.pitch *= UnityEngine.Random.Range(minPitch, maxPitch);
        s.source.PlayOneShot(s.clip);
        s.source.pitch = originalPitch;
    }


    public bool IsPlaying(string audio)
    {
        Sound s = Array.Find(sounds, sound => sound.name == audio);
        if (s != null && s.source.isPlaying)
        {
            Debug.Log("playing clip");
            return true;
        }
        else
        {
            Debug.Log("not playing");
            return false;
        }
    }

    public void Stop(string audioClip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == audioClip);
        if (s != null && s.source.isPlaying)
        {
            s.source.Stop();
        }
    }
}
