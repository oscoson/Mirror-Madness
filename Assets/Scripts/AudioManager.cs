using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip menuMusicClip;
    [SerializeField] AudioClip gameMusicClip;

    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";
    
    void Awake()
    {
        // if (instance == null)
        // {
        //     instance = this;

        //     DontDestroyOnLoad(gameObject);
        // } else {
        //     Destroy(gameObject);
        // }

        LoadVolume();
    }

    public void PlayMenuMusic()
    {
        audioSource.PlayOneShot(menuMusicClip);
    }

    public void PlayGameMusic()
    {
        audioSource.PlayOneShot(gameMusicClip);
    }

    void LoadVolume() // volume saved in SettingsMenu.cs
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        mixer.SetFloat(SettingsMenu.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat(SettingsMenu.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }
}
