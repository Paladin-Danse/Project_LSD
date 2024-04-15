using Constants;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance = null;   
    public Dungeon[] dungeons;
    private GameObject dungeonMapPrefab;    
    SelectedDungeonKeep selectedDungeonKeep;
    [HideInInspector] public int questId_add;
    [HideInInspector] public QuestType questType;
    [HideInInspector] public int target;
    [HideInInspector] public int curcount;
    [HideInInspector] public float missionTime;
    public TextMeshProUGUI missionTimeUI;
    public GameObject timeUI;
    public GameObject goldPrefab;
    GameObject dungeon;
    [HideInInspector] public int killedEneies = 0;
    [HideInInspector] public float totalDamage = 0;
    [HideInInspector] public float receivedDamage = 0;
    [HideInInspector] public int earnGold = 0;
    [HideInInspector] public DungeonMissionBoard missionBoard;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //else
        //{
        //    if (Instance != this)
        //        Destroy(this.gameObject);
        //}

        selectedDungeonKeep = FindObjectOfType<SelectedDungeonKeep>();
        missionBoard = FindObjectOfType<DungeonMissionBoard>();
        dungeonMapPrefab = dungeons[selectedDungeonKeep.mapNumber].Ddata.dungeonPrefab;
        questId_add = dungeons[selectedDungeonKeep.mapNumber].Ddata.QuestID;
        missionTime = 0f;
    }
    private void Start()
    {
        CreateMap();
        AddQuest();
        GameObject enemyChild = dungeon.transform.Find("EnemyGroup").gameObject;

        QuestManager.Instance.OnQuestCompleteCallback += delegate (int id)
        {
            enemyChild.SetActive(false);
        };
    }

    private void Update()
    {
        UpdateQuest();        
        missionTime += Time.deltaTime;
        float minutes = Mathf.Floor(missionTime / 60);
        float seconds = Mathf.RoundToInt(missionTime % 60);
        missionTimeUI.text = string.Format("{0:00}:{1:00}", minutes, seconds); ;
    }

    void CreateMap()
    {
        dungeon = Instantiate(dungeonMapPrefab, transform.position, transform.rotation);
    }

    public void AddQuest()
    {
        QuestManager.Instance.QuestStart(questId_add);
    }

    public void UpdateQuest()
    {
        QuestManager.Instance.DNotifyQuest(questType, target, curcount);        
    }    
}
