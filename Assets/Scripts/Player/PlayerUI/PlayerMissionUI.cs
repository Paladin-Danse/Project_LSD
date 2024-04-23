using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMissionUI : MonoBehaviour
{
    public GameObject missionBoard;
    public GameObject missionPanel;
    public TMP_Text missionTitle;
    public TMP_Text missionDescription;
    public TMP_Text missionProgress;
    public TMP_Text missionGoal;
    public TMP_Text missionTime;
    public GameObject missionCompletePanel;

    private void Awake()
    {
        QuestManager.Instance.OnQuestStartCallback += SetMissionBoard;
        QuestManager.Instance.OnQuestUpdateCallback += SetMissionProgress;
        QuestManager.Instance.OnQuestCompleteCallback += SetMissionComplete;
    }

    void SetMissionBoard(int questId) 
    {
        missionBoard.SetActive(true);
        missionPanel.SetActive(true);
        missionCompletePanel.SetActive(false);
        missionTitle.text = Database.Instance._quest._quest[questId].Name;
        missionDescription.text = Database.Instance._quest._quest[questId].Description;
        missionProgress.text = "0";
        missionGoal.text = Database.Instance._quest._quest[questId].Count.ToString();
    }

    void SetMissionProgress(int questId, int count) 
    {
        missionTitle.text = Database.Instance._quest._quest[questId].Name;
        missionDescription.text = Database.Instance._quest._quest[questId].Description;
        missionProgress.text = count.ToString();
        missionGoal.text = Database.Instance._quest._quest[questId].Count.ToString();
    }

    void SetMissionComplete(int questId)
    {
        missionCompletePanel.SetActive(true);
        missionPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        QuestManager.Instance.OnQuestStartCallback -= SetMissionBoard;
        QuestManager.Instance.OnQuestUpdateCallback -= SetMissionProgress;
        QuestManager.Instance.OnQuestCompleteCallback -= SetMissionComplete;
    }

    void RefreshTimeText() 
    {
        
    }
}
