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
    }

    private void Start()
    {
        audioSource.PlayOneShot(boomSound);
    }
}
