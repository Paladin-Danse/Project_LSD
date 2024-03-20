using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using UnityEngine;

[Serializable]
public enum EBuffType 
{
    None, Buff, Debuff
}

[Serializable]
public enum EBuffEndCondition 
{
    TimeOver,
    NoneStack,
    Trigger,
}


[Serializable]
public class Buff<T> where T : Stat
{
    public int buffID;
    public EBuffType type;
    public EBuffEndCondition endFlag;
    public float duration;
    public float remainingBufftime;
    public T basebuffStat;
    public T curBuffStat;

    public Action<int> OnTimeOver;
    public Action OnBuffChanged;

    public virtual void OnBuffed(BuffHandler<T> handler) 
    {
        curBuffStat.OverlapStats(basebuffStat);
        
        if(endFlag == EBuffEndCondition.TimeOver)
            OnTimeOver = handler.RemoveBuff;
    }

    public virtual void OnBuffOff() 
    {
        
    }

    public virtual void OverlapBuff() 
    {
        remainingBufftime = duration;
    }
}

public class StackableBuff<T> : Buff<T> where T : Stat
{
    public int stack;
    public int maxStack;
    public T statPerStack;
    public Action<int> OnStackZero;

    public override void OnBuffed(BuffHandler<T> handler)
    {
        base.OnBuffed(handler);

        OnBuffChanged = handler.UpdateBuff;

        if (endFlag == EBuffEndCondition.NoneStack) 
        {
            OnStackZero = handler.RemoveBuff;
            OnTimeOver = OnTimeOvered;
        } 
    }

    public override void OverlapBuff()
    {
        base.OverlapBuff();

        if(stack < maxStack) 
        {
            stack++;
            RefreshStackedStat();
        }

        remainingBufftime = duration;
    }

    void OnTimeOvered(int buffID) 
    {
        if(stack > 1) 
        {
            stack--;
            RefreshStackedStat();
        }
        else 
        {
            OnStackZero(buffID);
        }
    }

    void RefreshStackedStat() 
    {
        curBuffStat.OverlapStats(basebuffStat);
        
        for(int i = 0; i < stack; i++) 
        {
            curBuffStat.OverlapStats(statPerStack);
        }

        OnBuffChanged.Invoke();
    }
}

public class PeriodicBuff<T> : Buff<T> where T : Stat 
{
    public float period;

    public override void OnBuffed(BuffHandler<T> handler)
    {
        base.OnBuffed(handler);
        handler.InvokeRepeating("OnPeriod", 0f, period);
    }

    protected virtual void OnPeriod() 
    {
        // Do Something
    }
}


[DisallowMultipleComponent]
public class BuffHandler<T> : MonoBehaviour where T : Stat
{
    Dictionary<int, Buff<T>> buffDic;
    StatHandlerBase<T> handler;

    // Start is called before the first frame update
    private void Awake()
    {
        buffDic = new Dictionary<int, Buff<T>>();

        if(TryGetComponent<StatHandlerBase<T>>(out handler))
        {
            Debug.Log("Noooooo!");
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateBuffTimes();
    }

    protected void UpdateBuffTimes() 
    {
        foreach (var buff in buffDic)
        {
            buff.Value.remainingBufftime -= Time.deltaTime;

            if (buff.Value.remainingBufftime <= 0)
            {
                buff.Value.OnTimeOver.Invoke(buff.Key);
            }
        }
    }

    public void AddBuff(Buff<T> addBuff) 
    {
        if (buffDic.ContainsKey(addBuff.buffID)) 
        {
            buffDic[addBuff.buffID].OverlapBuff();
        }
        else 
        {
            buffDic.Add(addBuff.buffID, addBuff);
            addBuff.OnBuffed(this);
            handler.AddStatModifier(addBuff.curBuffStat);
        }
    }

    public void RemoveBuff(Buff<T> removeBuff) 
    {
        if (buffDic.ContainsKey(removeBuff.buffID))
        {
            buffDic.Remove(removeBuff.buffID);
        }
    }

    public void RemoveBuff(int buffID)
    {
        if (buffDic.ContainsKey(buffID))
        {
            buffDic.Remove(buffID);
        }
    }

    public void UpdateBuff() 
    {
        handler.UpdateStats();
    }

    public void ApplyBuff(Buff<T> applyBuff) 
    {
        
    }
}

