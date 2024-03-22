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
            stateMachine.Gun.StartCoroutine(ReloadCoroutine);
        }
        else
        {
            stateMachine.ChangeState(stateMachine.ReadyState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

    public IEnumerator Reload()
    {
        stateMachine.Gun.PlayClip(stateMachine.Gun.reload_start_AudioClip, stateMachine.Gun.reload_Volume);
        stateMachine.Gun.animator.SetInteger(stateMachine.Gun.animationData.reloadParameterHash, 1);

        yield return stateMachine.weaponReloadDelay;
        stateMachine.Gun.PlayClip(stateMachine.Gun.reload_end_AudioClip, stateMachine.Gun.reload_Volume);
        stateMachine.Gun.animator.SetInteger(stateMachine.Gun.animationData.reloadParameterHash, -1);
        stateMachine.curMagazine = stateMachine.maxMagazine;
        stateMachine.playerStateMachine_.player.playerUIEventInvoke();
        ReloadCoroutine = null;
        stateMachine.ChangeState(stateMachine.ReadyState);
    }
}
