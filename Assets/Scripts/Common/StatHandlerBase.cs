using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatHandlerBase <T> : MonoBehaviour where T : Stat
{
    [SerializeField]
    protected T baseStat;
    public List<T> statModifiers;

    [field : SerializeField]
    public T currentStat { get; protected set; }

    // Start is called before the first frame update

    protected void Awake()
    {
        InitStat();
        UpdateStats();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void InitStat()
    {
        // Init Stat with StatSO
    }

    public void AddStatModifier(T statModifier)
    {
        statModifiers.Add(statModifier);
        UpdateStats();
    }

    public void RemoveStatModifier(T statModifier)
    {
        statModifiers.Remove(statModifier);
        UpdateStats();
    }

    public void UpdateStats()
    {
        baseStat.statModifyType = StatModifyType.Override;
        currentStat.OverlapStats(baseStat);

        foreach (var statModifier in statModifiers.OrderBy(o => o.statModifyType))
        {
            currentStat.OverlapStats(statModifier);
        }
    }
}
