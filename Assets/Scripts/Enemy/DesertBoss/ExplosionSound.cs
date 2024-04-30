using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip explosionSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = SoundManager.instance.UISound.outputAudioMixerGroup;
    }

    private void Start()
    {
        audioSource.PlayOneShot(explosionSound);
    }
}
