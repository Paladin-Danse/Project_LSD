using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterBuff : Buff<CharacterStat>
{
    public override void OnBuffed(BuffHandler<CharacterStat> handler)
    {
        base.OnBuffed(handler);
        
        curBuffStat.characterStatFlag = basebuffStat.characterStatFlag;
    }
}

[Serializable]
public class CharacterStackableBuff : StackableBuff<CharacterStat> 
{
    public override void OnBuffed(BuffHandler<CharacterStat> handler)
    {
        base.OnBuffed(handler);

        curBuffStat.characterStatFlag = basebuffStat.characterStatFlag;
    }
}

[Serializable]
public class CharacterPeriodicBuff : PeriodicBuff<CharacterStat>
{
    public override void OnBuffed(BuffHandler<CharacterStat> handler)
    {
        base.OnBuffed(handler);

        curBuffStat.characterStatFlag = basebuffStat.characterStatFlag;
    }
}