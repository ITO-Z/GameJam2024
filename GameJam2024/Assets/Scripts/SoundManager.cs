using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public enum Sound{
        Soundtrack,
        Button,
        Buy,
        Expensive,
        Upgrade,
        Payday,
    }

    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start(){
        StopAllCoroutines();
        StartCoroutine(PlaySoundtrack());
        PlaySound(SoundAudioClipArray[0]);
    }

    public static void PlaySound(SoundAudioClip soundAudioClip){
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        if(soundAudioClip.sound == Sound.Soundtrack){

            audioSource.clip = soundAudioClip.audioClip[UnityEngine.Random.Range(0, soundAudioClip.audioClip.Length)];
            audioSource.Play();
        }
        else
            audioSource.PlayOneShot(soundAudioClip.audioClip[UnityEngine.Random.Range(0, soundAudioClip.audioClip.Length)]);
    }


    public SoundAudioClip[] SoundAudioClipArray;

    [Serializable]
    public class SoundAudioClip {
        public Sound sound;
        public AudioClip[] audioClip;
    }

    IEnumerator PlaySoundtrack(){
        while(true){
            PlaySound(SoundAudioClipArray[0]);
            float clipDuration = SoundAudioClipArray[0].audioClip.Length;
            yield return new WaitForSeconds(clipDuration);
            float timeToFade = 2f;
            float timeElapsed = 0f;
            if(timeElapsed < timeToFade){
                PlaySound(SoundAudioClipArray[0]);
                while(timeElapsed < timeToFade){
                    
                }
            }
        }
    }
}
