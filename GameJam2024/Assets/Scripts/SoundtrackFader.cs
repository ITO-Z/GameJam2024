using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackFader : MonoBehaviour
{
    public static SoundtrackFader soundtrackFader;
    [SerializeField] float fadeDuration = 10;
    AudioSource audioSource;
    bool fading;
    bool fadeOut;
    public SoundManager instance;

    void Awake()
    {
        if (soundtrackFader == null)
        {
            soundtrackFader = this;
            DontDestroyOnLoad(soundtrackFader);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;
        fading = true;
        StartCoroutine(StartFade(false));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(audioSource.clip.length - audioSource.time);
        if (audioSource.isPlaying && !fading && !fadeOut)
        {
            if ((audioSource.clip.length - audioSource.time) <= fadeDuration)
            {
                StartCoroutine(StartFade(true));
            }
        }
    }

    IEnumerator StartFade(bool fadeToZero)
    {
        fadeOut = fadeToZero;
        float t = 0;
        bool mata = false;
        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
            if (fadeOut && !mata && t > .5f)
            {
                mata = true;
                instance.PlaySoundtrack();
            }
            audioSource.volume = Mathf.Lerp(fadeToZero ? .2f : 0, fadeToZero ? 0 : .2f, t);
            yield return null;
        }

        if (!fadeOut)
            fading = false;
        if (fadeOut)
        {
            Destroy(gameObject);
        }
    }
}
