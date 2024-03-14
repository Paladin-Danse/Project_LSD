using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }
    public override void Update()
    {
        base.Update();
        Vector3 origin = stateMachine.player.transform.position;
        RaycastHit hit;
        LayerMask layerMask = stateMachine.player.layerMask_GroundCheck;
        float RayDistance = stateMachine.player.Data.airData.GroundCheckRay_Distance;

        Debug.DrawRay(origin, Vector3.down, Color.red,RayDistance);
        if (Physics.Raycast(new Ray(origin, Vector3.down), out hit, RayDistance, layerMask) && !stateMachine.player.isGrounded)
        {
            stateMachine.player.isGrounded = true;
            stateMachine.ChangeState(stateMachine.IdleState);
            Debug.Log("IsGround");
        }
    }
}
