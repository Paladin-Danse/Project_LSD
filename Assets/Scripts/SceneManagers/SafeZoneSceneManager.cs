using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SafeZoneSceneManager : SceneManagerBase
{
    Map map;
    PlayerCharacter playerCharacter;

    // Start is called before the first frame update
    void Start()
    {
        LoadScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene() 
    {
        // LoadData에서 인벤토리 데이터 등 불러옴
        Player.Instance.LoadData();

        LoadMap();

        LoadPlayerCharacter();

        playerCharacter.transform.position = map.spawnPoint.transform.position;

        // 플레이어 빙의 (플레이어 데이터 바인딩)

        // UI 로딩

    }

    void LoadMap() 
    {
        map = Addressables.InstantiateAsync("SafeZone").WaitForCompletion().GetComponent<Map>();
    }

    void LoadPlayerCharacter() 
    {
        var handle = Addressables.InstantiateAsync("PlayerCharacter");
        // handle.Completed += (handle) => { playerCharacter = handle.Result.GetComponent<PlayerCharacter>(); };

        // SceneLoader.Instance.loadingCanvasController.SetProgressText("캐릭터 로딩 중");

        handle.WaitForCompletion();
        // SceneLoader.Instance.loadingCanvasController.SetProgressText("캐릭터 로딩 완료");

        UIController.Instance.Pop();
        if (handle.Result.TryGetComponent<PlayerCharacter>(out playerCharacter))
        {
            UIController.Instance.Push<PlayerUI>("HUDCanvas", out Player.Instance.playerUI);
            Player.Instance.Possess(playerCharacter);
        }
    }

    public override void OnUnloadScene()
    {
        Player.Instance.SaveData();
        Player.Instance.UnPossess();
        ObjectPoolManager.Instance.ClearPools();
        UIController.Instance.Clear();
    }


}
