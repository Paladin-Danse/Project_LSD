using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DungeonSceneManager : SceneManagerBase
{
    Map map;
    PlayerCharacter playerCharacter;
    GameObject enemyGroup;

    private void Awake()
    {
        InitGame();
        SceneLoader.Instance.LoadCompleted();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadMap()
    {
        // SceneLoader.Instance.loadingCanvasController.SetProgressText("맵 로딩 중");

        map = GameObject.Instantiate(SelectedDungeonContext.Instance.selectedDungeonData.dungeonPrefab).GetComponent<Map>();
        enemyGroup = map.transform.Find("EnemyGroup").gameObject;

        // SceneLoader.Instance.loadingCanvasController.SetProgressText("맵 로딩 완료");
    }

    void LoadPlayerCharacter()
    {
        var handle = Addressables.InstantiateAsync("PlayerCharacter");
        // handle.Completed += (handle) => { playerCharacter = handle.Result; };

        // SceneLoader.Instance.loadingCanvasController.SetProgressText("캐릭터 로딩 중");

        handle.WaitForCompletion();
        if(handle.Result.TryGetComponent<PlayerCharacter>(out playerCharacter)) 
        {
            Debug.Log("PlayerCharacter 생성 완료");
        }
        // SceneLoader.Instance.loadingCanvasController.SetProgressText("캐릭터 로딩 완료");

    }

    void InitGame()
    {
        LoadMap();
        LoadPlayerCharacter();

        UIController.Instance.Clear();

        if (map.spawnPoint == null) Debug.Log("No SpawnPoint!");

        playerCharacter.transform.position = map.spawnPoint.transform.position;
        playerCharacter.transform.rotation = map.spawnPoint.transform.rotation;

        if (!UIController.Instance.Push<PlayerUI>("HUDCanvas", out Player.Instance.playerUI, EUIShowMode.Single))
        {
            Debug.Log("Failed to Create HUDCanvas!");
        }
        Player.Instance.Possess(playerCharacter);

        QuestManager.Instance.QuestStart(SelectedDungeonContext.Instance.selectedDungeonData.QuestID);
        QuestManager.Instance.OnQuestCompleteCallback += QuestEnd;
        DungeonTracker.Instance.InitTracker(SelectedDungeonContext.Instance.selectedDungeonData.QuestID);
    }

    public override void OnUnloadScene()
    {
        base.OnUnloadScene();
        Player.Instance.SaveData();
        Player.Instance.UnPossess();
        UIController.Instance.Clear();
    }

    public void QuestEnd(int questID) 
    {
        if(SelectedDungeonContext.Instance.selectedDungeonData.QuestID == questID) 
        {
            if(enemyGroup != null) { enemyGroup.SetActive(false); }
            QuestManager.Instance.OnQuestCompleteCallback -= QuestEnd;
        }
    }
}
