using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSelectedUISound : MonoBehaviour
{
    public void PlanetEnterS()
    {
        SoundManager.instance.PlanetEnterSound();
    }

    public void PlanetClickS()
    {
        SoundManager.instance.PlanetClickSound();
    }

    public void DungeonEntranceS()
    {
        SoundManager.instance.DungeonEntranceSound();
    }    
}
