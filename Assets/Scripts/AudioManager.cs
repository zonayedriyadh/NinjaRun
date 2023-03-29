using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
	public AudioMixerGroup mixerGroup;
    public Sound[] sounds;

    private bool enabledBGM = true;
    private bool enabledSFX = true;
    private Sound soundBG;


    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance =  FindObjectOfType<AudioManager>();
                instance.Init();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }


    public void Init()
    {
        foreach (Sound sound in instance.sounds)
        {
            sound.source = instance.gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.outputAudioMixerGroup = instance.mixerGroup;
        }
        RefreshSettings();
    }

    private void Awake()
    {
        AudioManager.Instance.Init();

    }
    void Start()
    {
        //RefreshSettings();
    }

    public void RefreshSettings()
    {
        EnableSFX(PlayerPrefs.GetInt("Sound", 1) == 1);
        EnableBGM(PlayerPrefs.GetInt("Music", 1) == 1);
    }

    public void SetBGOnOff(bool isOn)
    {
        int _isOn = isOn ? 1 : 0;
        PlayerPrefs.SetInt("Music", _isOn);

        PlayerPrefs.Save();
        RefreshSettings();
    }

    public void SetSFXOnOff(bool isOn)
    {
        int _isOn = isOn ? 1 : 0;
        PlayerPrefs.SetInt("Sound", _isOn);

        PlayerPrefs.Save();
        RefreshSettings();
    }
    public void EnableBGM(bool enable)
    {
        enabledBGM = enable;
        if (soundBG != null)
        {
            soundBG.source.mute = !enabledBGM;
        }
    }

    public void EnableSFX(bool enable)
    {
        enabledSFX = enable;
        foreach (Sound sound in sounds)
        {
            if (sound != soundBG)
            {
                sound.source.mute = !enabledSFX;
            }
        }
    }

    public bool isEnabledBGM()
    {
        return enabledBGM;
    }

    public bool isEnabledSFX()
    {
        return enabledSFX;
    }

    #region BGM

    public void PlayBGM(string soundName)
    {
        PlayBGM(soundName, 1f);
    }

    public void PlayBGM(string soundName, float volume)
    {
        StopBGM();
        soundBG = Array.Find(sounds, item => item.name == soundName);
        if (soundBG == null)
        {
            Debug.LogWarning("Sound not found!" + soundName);
            return;
        }
        soundBG.source.volume = volume;
        soundBG.source.loop = true;
        soundBG.source.mute = !enabledBGM;
        soundBG.source.Play();
    }

    public void StopBGM()
    {
        if (soundBG != null)
        {
            soundBG.source.Stop();
            soundBG = null;
        }
    }

    public bool IsPlayingBGM()
    {
        if (soundBG != null)
        {
            return soundBG.source.isPlaying;
        }
        return false;
    }

    #endregion


    #region SFX

    public void Play(string soundName)
    {
        Play(soundName, false, 1f);
    }

    public void Play(string soundName, bool loop)
    {
        Play(soundName, loop, 1f);
    }

    public void Play(string soundName, bool loop, float volume)
    {
        Sound sound = Array.Find(sounds, item => item.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning("Sound not found!" + soundName);
            return;
        }
        sound.source.volume = volume;
        sound.source.loop = loop;
        sound.source.mute = !enabledSFX;
        sound.source.Play();
    }

    public void PlayOneShot(string soundName)
    {
        PlayOneShot(soundName, 1);
    }

    public void PlayOneShot(string soundName, float volume)
    {
        Sound sound = Array.Find(sounds, item => item.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning("Sound not found! " + soundName);
            return;
        }
        sound.source.volume = volume;
        sound.source.loop = false;
        sound.source.mute = !enabledSFX;
        sound.source.PlayOneShot(sound.clip);
    }

    public void Stop(string soundName)
    {
        Sound sound = Array.Find(sounds, item => item.name == soundName);
        if (sound != null)
        {
            sound.source.Stop();
        }
    }

    public bool IsPlaying(string soundName)
    {
        Sound sound = Array.Find(sounds, item => item.name == soundName);
        if (sound != null)
        {
            return sound.source.isPlaying;
        }
        return false;
    }

    #endregion
}
