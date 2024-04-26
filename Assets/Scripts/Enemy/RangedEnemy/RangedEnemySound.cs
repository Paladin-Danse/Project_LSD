using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemySound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip ShotSound;
    public AudioClip WalkSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = SoundManager.instance.UISound.outputAudioMixerGroup;
    }

    void GunShotSound()
    {
        audioSource.PlayOneShot(ShotSound);
    }

    void REWalkSound()
    {
        audioSource.PlayOneShot(WalkSound);
    }
}
