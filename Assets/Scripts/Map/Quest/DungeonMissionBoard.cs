using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonMissionBoard : MonoBehaviour
{
    public GameObject missionBoard;
    public TextMeshProUGUI labelTxt;
    public TextMeshProUGUI descriptionTxt;
    public TextMeshProUGUI curCount;
    public TextMeshProUGUI fullCountTxt;
    QuestManager questManager;
    Database database;

    private void Awake()
    {
        questManager = QuestManager.Instance;    
        database = Database.Instance;
    }

    private void Start()
    {
        QuestManager.Instance.OnQuestStartCallback += delegate (int id)
        {
            labelTxt.text = database._quest._quest[id].Name;
            descriptionTxt.text = database._quest._quest[id].Description;
            fullCountTxt.text = database._quest._quest[id].Count.ToString();
            Debug.Log("Start " + id);
        };

        QuestManager.Instance.OnQuestUpdateCallback += delegate (int id, int count)
        {
            Debug.Log("Update " + id + "   - " + count);
        };

        QuestManager.Instance.OnQuestCompleteCallback += delegate (int id)
        {
            Debug.Log("Complete " + id);
        };
    }

    private void Update()
    {
        if (questManager._ongoingQuests.Count > 0)
        {
            missionBoard.SetActive(true);
        }
        else
        {
            missionBoard.SetActive(false);
        }
    }
}
