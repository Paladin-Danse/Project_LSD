using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuestDB
{
    public Dictionary<int, QuestData> _quest = new ();
    
    public QuestDB()
    {
        var res = Resources.Load<QuestSO>("DB/QuestSO");
        var questSo = Object.Instantiate(res);
        var entities = questSo.Entities;
        
        if(entities == null || entities.Count <= 0)
            return;

        var entityCount = entities.Count;
        for (int i = 0; i < entityCount; i++)
        {
            var quest = entities[i];
            
            if (_quest.ContainsKey(quest.ID))
                _quest[quest.ID] = quest;
            else
                _quest.Add(quest.ID, quest);
        }
    }

    public QuestData Get(int id)
    {
        if (_quest.ContainsKey(id))
            return _quest[id];
        
        
        return null;
    }

    public IEnumerator DbEnumerator()
    {
        return _quest.GetEnumerator();
    }
}
