using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public Dungeon[] dungeons;
    private GameObject dungeonMapPrefab;
    SelectedDungeonKeep selectedDungeonKeep;
    public int questId_add;
    public QuestType questType;
    public int target;
    public int curcount;
    public static int amountGold = 0;

    private void Awake()
    {
        selectedDungeonKeep = FindObjectOfType<SelectedDungeonKeep>();
        dungeonMapPrefab = dungeons[selectedDungeonKeep.mapNumber].Ddata.dungeonPrefab;
        questId_add = dungeons[selectedDungeonKeep.mapNumber].Ddata.QuestID;
    }
    private void Start()
    {
        CreateMap();
        AddQuest();
    }

    private void Update()
    {
        UpdateQuest();
    }

    void CreateMap()
    {
        Instantiate(dungeonMapPrefab, transform.position, transform.rotation);
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
