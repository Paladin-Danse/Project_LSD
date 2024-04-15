using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Database : SingletoneBase<Database>
{        
    public QuestDB _quest;
    public static QuestDB Quest
    {
        get
        {
            if (Instance._quest == null)
                Instance._quest = new QuestDB();
            return Instance._quest;
        }
    }
}
