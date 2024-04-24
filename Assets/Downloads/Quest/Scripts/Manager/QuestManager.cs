using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class QuestManager : SingletoneBase<QuestManager>
{
    public Dictionary<int, Quest> _ongoingQuests = new ();
    private HashSet<int> _completeQuests = new ();

    public event Action<int> OnQuestStartCallback;
    public event Action<int, int> OnQuestUpdateCallback;
    public event Action<int> OnQuestCompleteCallback;

    private Dictionary<QuestType, List<QuestData>> _subscribeQuests = new();        

    #region Auto Manage

    public void SubscribeQuest(int questId)
    {
        Debug.Log("SubscribeQuest " + questId);
        
        var questData = Database.Quest.Get(questId);

        if (_subscribeQuests.ContainsKey(questData.Type) == false)
            _subscribeQuests[questData.Type] = new List<QuestData>();
        
        _subscribeQuests[questData.Type].Add(questData);
    }
    
    public void UnsubscribeQuest(int questId)
    {
        Debug.Log("UnsubscribeQuest " + questId);
        var questData = Database.Quest.Get(questId);

        if (_subscribeQuests.ContainsKey(questData.Type) == false)
            return;
        
        _subscribeQuests[questData.Type].Remove(questData);
    }
    
    public void DNotifyQuest(QuestType type, int target, int count)
    {
        if (_subscribeQuests.ContainsKey(type) == false)
            return;

        var filteredQuests = _subscribeQuests[type];
        var targetQuests = filteredQuests.FindAll(q => q.Target == target);
        foreach (var quest in targetQuests)
            DQuestUpdate(quest.ID, count);
    }

    public void NotifyQuest(QuestType type, int target, int count)
    {
        if (_subscribeQuests.ContainsKey(type) == false)
            return;

        var filteredQuests = _subscribeQuests[type];
        var targetQuests = filteredQuests.FindAll(q => q.Target == target);
        foreach (var quest in targetQuests)
            QuestUpdate(quest.ID, count);
    }

    #endregion

    #region Quest Controll

    public void QuestStart(int questId)
    {
        if(IsClear(questId))
            return;
        
        var quest = new Quest(questId);
        quest.Start();
        
        if(_ongoingQuests.ContainsKey(questId))
            _ongoingQuests.Remove(questId);
        
        _ongoingQuests.Add(questId, quest);

        SubscribeQuest(questId);
        
        OnQuestStartCallback?.Invoke(questId);
    }
    
    
    public void DQuestUpdate(int questId, int amount)
    {
        if(_ongoingQuests.ContainsKey(questId) == false)
            return;

        var questData = Database.Quest.Get(questId);
        
        int currentCount = _ongoingQuests[questId].Update(amount);
        
        OnQuestUpdateCallback?.Invoke(questId, amount);

        if (currentCount >= questData.Count)
            DQuestClear(questId);
    }

    public void QuestUpdate(int questId, int amount)
    {
        if (_ongoingQuests.ContainsKey(questId) == false)
            return;

        var questData = Database.Quest.Get(questId);

        int currentCount = _ongoingQuests[questId].Update(amount);

        OnQuestUpdateCallback?.Invoke(questId, amount);

        if (currentCount >= questData.Count)
            QuestClear(questId);
    }


    public void QuestClear(int questId)
    {
        if(_ongoingQuests.ContainsKey(questId) == false)
            return;

        _ongoingQuests[questId].Complete();
        _ongoingQuests.Remove(questId);

        _completeQuests.Add(questId);
        
        OnQuestCompleteCallback?.Invoke(questId);
    }

    public void DQuestClear(int questId)
    {
        if (_ongoingQuests.ContainsKey(questId) == false)
            return;

        _ongoingQuests[questId].Complete();
        _ongoingQuests.Remove(questId);        

        OnQuestCompleteCallback?.Invoke(questId);
    }



    public bool IsClear(int id)
    {
        return _completeQuests.Contains(id);
    }

    #endregion
    
}