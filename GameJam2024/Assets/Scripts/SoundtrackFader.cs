using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackFader : MonoBehaviour
{
    [SerializeField] float fadeDuration = 10;
    AudioSource audioSource;
    bool fading;
    bool fadeOut;
    public SoundManager instance;

    void Awake()
    {
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
            audioSource.volume = Mathf.Lerp(fadeToZero ? .5f : 0, fadeToZero ? 0 : .5f, t);
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
