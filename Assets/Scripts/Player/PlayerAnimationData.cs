using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationData
{
    [SerializeField] private string groundParameterName = "IsGrounded";
    [SerializeField] private string crouchParameterName = "IsCrouching";
    [SerializeField] private string DirectionParameterName = "Direction";
    [SerializeField] private string SpeedParameterName = "Speed";


    public int GroundParameterHash { get; private set; }
    public int CrouchParameterHash { get; private set; }
    public int DirectionParameterHash { get; private set; }
    public int SpeedParameterHash { get; private set; }

    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        CrouchParameterHash = Animator.StringToHash(crouchParameterName);
        DirectionParameterHash = Animator.StringToHash(DirectionParameterName);
        SpeedParameterHash = Animator.StringToHash(SpeedParameterName);
    }
}
