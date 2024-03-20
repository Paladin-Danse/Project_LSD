using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunReloadState : GunBaseState
{
    IEnumerator ReloadCoroutine = null;
    public GunReloadState(GunStateMachine gunStateMachine) : base(gunStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (ReloadCoroutine == null)
        {
            ReloadCoroutine = Reload();
        }
    }

    public IEnumerator Reload()
    {
        yield return stateMachine.weaponReloadDelay;
        stateMachine.curMagazine = stateMachine.maxMagazine;
    }
}
