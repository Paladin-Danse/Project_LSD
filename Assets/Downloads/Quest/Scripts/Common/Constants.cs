using UnityEngine;

namespace Constants
{
    public enum ItemType
    {
        Weapon,
        Armor,
        Consume
    }
        
    public enum ItemGrade 
    {
        Normal, // 0
        Magic,  // 1
        Rare,   // 2
        Unique  // 3
    }


    public enum QuestType
    {
        GetItem,
        UseItem,
        KillMonster,
        TalkNpc,
        EnterMap,
        ClearMap,
        OpenUI,
    }

    public enum QuestState
    {
        Waite,
        Progress,
        Complete
    }

    public class Colors
    {
        public static readonly Color[] ItemGrade = new[]
        {
            new Color(0.7f, 0.68f, 0.68f), // Normal
            new Color(0.23f,0.56f,0.95f), // Magic
            new Color(0.95f, 0.67f, 0.24f), // Rare
            new Color(1, 0.22f, 0.96f) // Unique
        };
    }
}