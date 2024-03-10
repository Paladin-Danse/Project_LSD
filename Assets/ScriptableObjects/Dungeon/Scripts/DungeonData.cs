using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New DungeonData", menuName = "new Data/Dungeon")]
public class DungeonData : ScriptableObject
{
    [Header("DungeonInfo")]
    public string dungeonName;
    public Sprite dungeonImage;
    public string dungeonEx;
    public int dungeonNumber;
}
