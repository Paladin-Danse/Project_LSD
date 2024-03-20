using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponBuff : Buff<WeaponStat>
{

}

[Serializable]
public class WeaponStackableBuff : StackableBuff<WeaponStat> 
{

}

[Serializable]
public class WeaponPeriodicBuff : PeriodicBuff<WeaponStat> 
{

}
