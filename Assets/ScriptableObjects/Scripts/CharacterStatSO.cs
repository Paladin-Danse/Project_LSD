using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultStatData", menuName = "Stats/CharacterStat/Default", order = 0)]
public class CharacterStatSO : ScriptableObject
{
    public CharacterStat characterStat;
}
