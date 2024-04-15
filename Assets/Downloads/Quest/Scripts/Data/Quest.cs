using Constants;
using UnityEngine;

public class Quest
{
    [SerializeField] private int _questId;
    [SerializeField] private int _questProgress;
    [SerializeField] private QuestState _questState;

    public int QuestId
    {
        get => _questId;
        private set => _questId = value;
    }

    public int QuestProgress
    {
        get => _questProgress;
        private set => _questProgress = value;
    }

    public QuestState QuestState
    {
        get => _questState;
        private set => _questState = value;
    }

    
    public Quest(int questId)
    {
        _questId = questId;
        _questProgress = 0;
        _questState = QuestState.Waite;
    }
    
    public Quest(int questId, int questProgress, QuestState questState)
    {
        _questId = questId;
        _questProgress = questProgress;
        _questState = questState;
    }

    public void Start()
    {
        _questState = QuestState.Progress;
    }
    
    public int Update(int amount)
    {
        _questProgress += amount;
        return _questProgress;
    }

    public void Complete()
    {
        _questState = QuestState.Complete;
    }
}