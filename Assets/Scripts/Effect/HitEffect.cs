using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public ParticleSystem particleSystem;

    private void Awake()
    {
        var action = particleSystem.main;
        action.stopAction = ParticleSystemStopAction.Callback;
    }

    private void OnParticleSystemStopped()
    {
        ObjectPoolManager.Instance.TryPush(gameObject);
    }
}
