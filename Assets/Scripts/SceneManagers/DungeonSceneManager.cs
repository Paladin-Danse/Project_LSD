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

    private void Awake()
    {
        InitGame();
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

        if (!UIController.Instance.Push<PlayerUI>("HUDCanvas", out Player.Instance.playerUI, EUIShowMode.Single))
        {
            Debug.Log("Failed to Create HUDCanvas!");
        }
        Player.Instance.Possess(playerCharacter);

        if (!UIController.Instance.Push<PlayerMissionUI>("MissionCanvas", out PlayerMissionUI playerMissionUI, EUIShowMode.Additive))
        {
            Debug.Log("Failed to Create MissionCanvas!");
        }

        QuestManager.Instance.QuestStart(SelectedDungeonContext.Instance.selectedDungeonData.QuestID);
        DungeonTracker.Instance.InitTracker(SelectedDungeonContext.Instance.selectedDungeonData.QuestID);
    }

    public override void OnUnloadScene()
    {
        base.OnUnloadScene();
        Player.Instance.SaveData();
        Player.Instance.UnPossess();
        ObjectPoolManager.Instance.ClearPools();
        UIController.Instance.Clear();
    }
}
