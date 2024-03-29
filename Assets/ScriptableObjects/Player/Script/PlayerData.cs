using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new PlayerData", menuName = "new Data/Player")]
public class PlayerData : ScriptableObject
{
    //[Header("CurrentStat")]
    //public CharacterStat stat;
    [Header("Ground")]
    public PlayerGroundData groundData;
    [Header("Air")]
    public PlayerAirData airData;
    [Header("ETC")]
    public float LookRotateSpeed;
    public float LookRotateModifier;
    public float UpdownMaxAngle;
}
