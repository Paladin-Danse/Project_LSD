using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectileBoomSound : MonoBehaviour
{
    public AudioClip boomSound;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = SoundManager.instance.UISound.outputAudioMixerGroup;
    }

    private void Start()
    {
        audioSource.PlayOneShot(boomSound);
    }
}
