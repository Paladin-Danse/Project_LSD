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
    public WaitForSeconds whileRestTimeSeconds;
    public IEnumerator ShotCoroutine = null;
    public IEnumerator RecoilCoroutine = null;
    public GunReadyState ReadyState { get; }
    public GunEmptyState EmptyState { get; }
    public GunReloadState ReloadState { get; }
    public int maxMagazine;
    public string maxMagazineText { get { return maxMagazine.ToString(); } }
    public int curMagazine;
    public string curMagazineText { get { return curMagazine.ToString(); } }
    public bool isEmpty => curMagazine <= 0;
    public float targetRecoil = 0f;
    public float curRecoil = 0f;
    public float maxRecoil;
    public float defaultSpread = 0f;
    public float maxSpread;
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
}
