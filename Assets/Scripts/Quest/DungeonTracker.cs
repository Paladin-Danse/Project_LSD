using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTracker : SingletoneBase<DungeonTracker>
{
    bool isQuestInProgress = false;
    public int curQuestID { get; private set; }
    public float missionTime = 0;
    int _killedEnemies;
    public int killedEnemies { get { return _killedEnemies; } set { if (isQuestInProgress) { _killedEnemies = value; } } }
    float _totalDamage;
    public float totalDamage { get { return _totalDamage; } set { if (isQuestInProgress) { _totalDamage = value; } } }

    float _receivedDamage;
    public float receivedDamage { get { return _receivedDamage; } set { if (isQuestInProgress) { _receivedDamage = value; } } }

    int _earnGold;
    public int earnGold { get { return _earnGold; } set { if (isQuestInProgress) { _earnGold = value; } } }

    public event Action OnTrackerUpdated;
    public event Action OnTimerUpdatedPerSeconds;

    private void Start()
    {
        InvokeRepeating("TimerUpdate", 0, 1f);
    }

    private void Update()
    {
        if (isQuestInProgress) 
        {
            missionTime += Time.deltaTime;
        }
    }

    public void InitTracker(int questId) 
    {
        QuestManager.Instance.OnQuestCompleteCallback += QuestEnd;
        missionTime = 0;
        _killedEnemies = 0;
        _totalDamage = 0;
        _receivedDamage = 0;
        _earnGold = 0;
        isQuestInProgress = true;
    }

    public void QuestEnd(int questID) 
    {
        QuestManager.Instance.OnQuestCompleteCallback -= QuestEnd;
        isQuestInProgress = false;
    }

    public void TimerUpdate() 
    {
        if (isQuestInProgress)
        {
            OnTimerUpdatedPerSeconds?.Invoke();
        }
    }
}
