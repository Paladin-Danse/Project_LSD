using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationData
{
    [SerializeField] private string movementSpeedParameterName = "Speed";
    [SerializeField] private string fireParameterName = "Fire";
    [SerializeField] private string reloadParameterName = "Reload";
    [SerializeField] private string sightParameterName = "Sight";


    public int movementSpeedParameterHash { get; private set; }
    public int fireParameterHash { get; private set; }
    public int reloadParameterHash { get; private set; }
    public int sightParameterHash { get; private set; }

    public void Initialize()
    {
        movementSpeedParameterHash = Animator.StringToHash(movementSpeedParameterName);
        fireParameterHash = Animator.StringToHash(fireParameterName);
        reloadParameterHash = Animator.StringToHash(reloadParameterName);
        sightParameterHash = Animator.StringToHash(sightParameterName);
    }
}
