using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class QuestTester : MonoBehaviour
{
    public int questId_add;
    public QuestType questType;
    public int target;
    public int count;

    public void AddQuest()
    {
        QuestManager.Instance.QuestStart(questId_add);
    }
    
    public void UpdateQuest()
    {
        QuestManager.Instance.NotifyQuest(questType, target, count);
    }
    
    private void Start()
    {
        QuestManager.Instance.OnQuestStartCallback += delegate(int id)
        {
            Debug.Log("Start " + id);
        };
        
        QuestManager.Instance.OnQuestUpdateCallback += delegate(int id, int count)
        {
            Debug.Log("Update " + id + "   - " + count);
        };
        
        QuestManager.Instance.OnQuestCompleteCallback += delegate(int id)
        {
            Debug.Log("Complete " + id);
        };
    }
}
