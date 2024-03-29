using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAnimationData
{
    [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string walkParameterName = "Walk";
    [SerializeField] private string runParameterName = "Run";    

    [SerializeField] private string attackParameterName = "@Attack";    
    [SerializeField] private string baseAttackParameterName = "BaseAttack";
    [SerializeField] private string bigAttackParameterName = "BigAttack";
    [SerializeField] private string smallAttackParameterName = "SmallAttack";


    public int GroundParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int BigAttackParameterHash { get; private set; }
    public int SmallAttackParameterHash { get; private set; }

    public int AttackParameterHash { get; private set; }    
    public int BaseAttackParameterHash { get; private set; }

    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);

        BigAttackParameterHash = Animator.StringToHash(bigAttackParameterName);
        SmallAttackParameterHash = Animator.StringToHash(smallAttackParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);        
        BaseAttackParameterHash = Animator.StringToHash(baseAttackParameterName);
    }
}
