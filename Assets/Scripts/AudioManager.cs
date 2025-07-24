using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    #region Audio Sources
    public AudioMixer audioMixer;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    #endregion

    #region Audio Clips
    public AudioClip[] musicTracks;
    public AudioClip[] sfxClips;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(int index)
    {
        if (bgmSource.clip == musicTracks[index] && bgmSource.isPlaying) return;

        bgmSource.clip = musicTracks[index];
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(int index)
    {
        sfxSource.PlayOneShot(sfxClips[index]);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20); // volume: 0.0001 to 1.0
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

}
