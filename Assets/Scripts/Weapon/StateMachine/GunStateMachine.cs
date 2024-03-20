using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GunStateMachine : StateMachine
{
    public Weapon Gun { get; }

    public WaitForSeconds weaponAttackDelay;
    public WaitForSeconds weaponReloadDelay;
    public IEnumerator ShotCoroutine = null;
    public GunReadyState ReadyState { get; }
    public GunEmptyState EmptyState { get; }
    public GunReloadState ReloadState { get; }
    public int maxMagazine;
    public int curMagazine;
    public bool isEmpty => curMagazine <= 0;
    public float addRecoil = 0f;
    public float maxRecoil;
    public float recoveryRecoil;
    public PlayerStateMachine playerStateMachine_;
    public GunStateMachine(Weapon Gun)
    {
        this.Gun = Gun;
        
        ReadyState = new GunReadyState(this);
        EmptyState = new GunEmptyState(this);
        ReloadState = new GunReloadState(this);
    }
    public void DebugCurrentState()
    {
        Debug.Log(currentState);
    }
    public void OnRecoil()
    {
        addRecoil = math.min(addRecoil + Gun.currentStat.recoil, maxRecoil);
    }
}
