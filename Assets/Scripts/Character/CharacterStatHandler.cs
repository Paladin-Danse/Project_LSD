using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
public class CharacterStatHandler : MonoBehaviour
{
    [SerializeField]
    CharacterStatSO baseStatSO;

    [SerializeField]
    CharacterStat baseStat;
    public List<CharacterStat> statModifiers;
    public CharacterStat currentStat { get; private set; }

    private void Awake()
    {
        InitStat();
        UpdateCharacterStat();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCharacterStatModifier(CharacterStat statModifier) 
    {
        statModifiers.Add(statModifier);
        UpdateCharacterStat();
    }

    public void RemoveCharacterStatModifier(CharacterStat statModifier) 
    {
        statModifiers.Remove(statModifier);
        UpdateCharacterStat();
    }

    void InitStat() 
    {
        if (baseStatSO != null) 
        {
            baseStat = baseStatSO.characterStat;
        }
    }

    void UpdateCharacterStat() 
    {
        currentStat.Override(baseStat);

        foreach(var statModifier in statModifiers.OrderBy(o => o.statModifyType)) 
        {
            if(statModifier.statModifyType == StatModifyType.Add) 
            {
                currentStat.Add(statModifier);
            }
            else if(statModifier.statModifyType == StatModifyType.Multiply) 
            {
                currentStat.Multiply(statModifier);
            }
            else if(statModifier.statModifyType == StatModifyType.Override) 
            {
                currentStat.Override(statModifier);
            }
        }
    }
}
