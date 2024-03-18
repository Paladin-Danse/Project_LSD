using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStateMachine : StateMachine
{
    public Weapon Gun { get; }

    public WaitForSeconds weaponAttackDelay;
    public IEnumerator ShotCoroutine = null;
    public GunReadyState ReadyState { get; }

    public GunStateMachine(Weapon Gun)
    {
        this.Gun = Gun;
        ReadyState = new GunReadyState(this);
    }
}
