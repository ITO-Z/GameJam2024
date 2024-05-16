using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySFX : MonoBehaviour
{
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if ((audioSource.clip.length - audioSource.time) <= 0)
            Destroy(gameObject);
    }
}
