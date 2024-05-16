using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public enum Sound
    {
        Soundtrack,
        Button,
        Buy,
        Expensive,
        Upgrade,
        Payday,
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StopAllCoroutines();
        PlaySound(SoundAudioClipArray[0]);
    }
    public void PlaySoundtrack()
    {
        PlaySound(SoundAudioClipArray[0]);
    }
    public void Play(int type)
    {
        PlaySound(SoundAudioClipArray[type]);
    }
    void PlaySound(SoundAudioClip soundAudioClip)
    {
        GameObject soundGameObject = new GameObject((soundAudioClip.sound == Sound.Soundtrack ? "Soundtrack" : "Sound"));
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        if (soundAudioClip.sound == Sound.Soundtrack)
        {
            var o = soundGameObject.AddComponent<SoundtrackFader>();
            o.instance = this;
            audioSource.clip = soundAudioClip.audioClip[UnityEngine.Random.Range(0, soundAudioClip.audioClip.Length)];
            audioSource.Play();
        }
        else
        {
            audioSource.clip = soundAudioClip.audioClip[UnityEngine.Random.Range(0, soundAudioClip.audioClip.Length)];
            audioSource.Play();
            soundGameObject.AddComponent<DestroySFX>();
        }
    }


    public SoundAudioClip[] SoundAudioClipArray;

    [Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public AudioClip[] audioClip;
    }
}
