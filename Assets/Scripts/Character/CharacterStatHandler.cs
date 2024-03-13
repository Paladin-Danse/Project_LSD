using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
public class CharacterStatHandler : StatHandlerBase<CharacterStat>
{
    [SerializeField]
    CharacterStatSO baseStatSO;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void InitStat() 
    {
        if (baseStatSO != null) 
        {
            baseStat = baseStatSO.characterStat;
        }
    }
}
