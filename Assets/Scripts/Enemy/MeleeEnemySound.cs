using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemySound : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip WalkSound;
    public AudioClip PunchSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnWalkSound()
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(WalkSound);
    }
    void OnPunchSound()
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(PunchSound);
    }
}
