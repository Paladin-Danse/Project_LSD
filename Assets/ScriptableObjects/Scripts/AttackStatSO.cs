using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultAttackData", menuName = "Stats/AttackStat/Default", order = 0)]
public class AttackDataSO : ScriptableObject
{
    public AttackStat attackStat;
}
