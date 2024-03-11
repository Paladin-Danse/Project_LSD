using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonTooltip : MonoBehaviour
{
    public TextMeshProUGUI dungeonDisplayName;
    public Image dungeonImage;
    public TextMeshProUGUI dungeonDescription;
    Dungeon dungeon;
    
    public void OnTooltip(Dungeon dungeon)
    {
        this.dungeon = dungeon;
        dungeonDisplayName.text = dungeon.Ddata.dungeonName;
        dungeonImage.sprite = dungeon.Ddata.dungeonImage;
        dungeonDescription.text = dungeon.Ddata.dungeonEx;
    }
}
