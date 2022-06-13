using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager> {
    public enum AudioType
    {
        BGM = 0,
        SFX
    }
    public enum SFX_CLIP
    {
        Get_Coin = 0,
        Get_Gem,
        Get_Invincible,
        Get_Item,
        Mon_Die
    }
    public enum BGM_CLIP
    {
        BGM_01 = 0
    }
    AudioSource [] m_audio = new AudioSource[2];
    [SerializeField]
    AudioClip[] m_sfxClip;
    [SerializeField]
    AudioClip[] m_bgmClip;

    float m_fBGM_Volume;

    protected override void OnStart()
    {
        base.OnStart();
        m_audio[(int)AudioType.BGM] = gameObject.AddComponent<AudioSource>() as AudioSource;
        m_audio[(int)AudioType.BGM].loop = true;
        m_audio[(int)AudioType.BGM].playOnAwake = false;
        m_audio[(int)AudioType.BGM].rolloffMode = AudioRolloffMode.Linear;

        m_audio[(int)AudioType.SFX] = gameObject.AddComponent<AudioSource>() as AudioSource;
        m_audio[(int)AudioType.SFX].loop = false;
        m_audio[(int)AudioType.SFX].playOnAwake = false;
        m_audio[(int)AudioType.SFX].rolloffMode = AudioRolloffMode.Linear;

        if (PlayerPrefs.HasKey("BGM_Volume"))
        {
            m_fBGM_Volume = PlayerPrefs.GetFloat("BGM_Volume");
            SetVolumeBGM(m_fBGM_Volume);
        }

    }
    public void SetVolumeBGM(float volume)
    {
        m_audio[(int)AudioType.BGM].volume = volume;        
    }
    public void SetVolumeSFX(float volume)
    {
        m_audio[(int)AudioType.SFX].volume = volume;
    }
    public void SetVolume(float volume)
    {
        for(int i = 0; i < m_audio.Length; i++)
        {
            m_audio[i].volume = volume;
        }
    }
    public void SetMute()
    {
        for (int i = 0; i < m_audio.Length; i++)
        {
            m_audio[i].mute = true;
        }
    }
    public void OffMute()
    {
        for (int i = 0; i < m_audio.Length; i++)
        {
            m_audio[i].mute = false;
        }
    }
    public void PlayBGM(BGM_CLIP bgm)
    {
        m_audio[(int)AudioType.BGM].clip = m_bgmClip[(int)bgm];
        m_audio[(int)AudioType.BGM].Play();
    }
    public void PlaySFX(SFX_CLIP sfx)
    {
        m_audio[(int)AudioType.SFX].clip = m_sfxClip[(int)sfx];
        m_audio[(int)AudioType.SFX].Play();
    }
    public void Stop(AudioType type)
    {
        m_audio[(int)type].Stop();
    }
    public void Stop()
    {
        for (int i = 0; i < m_audio.Length; i++)
        {
            m_audio[i].Stop();
        }
    }
}
