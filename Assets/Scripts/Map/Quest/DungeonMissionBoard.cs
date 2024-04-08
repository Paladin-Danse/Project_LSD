using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonMissionBoard : MonoBehaviour
{
    public GameObject missionBoard;
    public TextMeshProUGUI labelTxt;
    public TextMeshProUGUI descriptionTxt;
    public TextMeshProUGUI curCountTxt;
    public TextMeshProUGUI fullCountTxt;
    public GameObject missionCompleteImage;
    public GameObject DungeonCompletePanel;
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
            curCountTxt.text = 0.ToString();
            Debug.Log("Start " + id);
        };

        QuestManager.Instance.OnQuestUpdateCallback += delegate (int id, int count)
        {            
            curCountTxt.text = count.ToString();//questManager._ongoingQuests[id].QuestProgress.ToString();
            Debug.Log("Update " + id + "   - " + count);
        };

        QuestManager.Instance.OnQuestCompleteCallback += delegate (int id)
        {
            missionCompleteImage.SetActive(true);
            DungeonCompletePanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Complete " + id);
        };
    }

    private void Update()
    {
        if (questManager._ongoingQuests.Count > 0)
        {
            //missionCompleteImage.SetActive(false);
            missionBoard.SetActive(true);
        }
        else
        {
            missionBoard.SetActive(false);
        }
    }
}
