using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterBuff : Buff<CharacterStat>
{

}

[Serializable]
public class CharacterStackableBuff : StackableBuff<CharacterStat> 
{

}

[Serializable]
public class CharacterPeriodicBuff : PeriodicBuff<CharacterStat>
{

}