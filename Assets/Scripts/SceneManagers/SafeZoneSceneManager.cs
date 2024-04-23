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
        // LoadData���� �κ��丮 ������ �� �ҷ���
        Player.Instance.LoadData();

        LoadMap();

        LoadPlayerCharacter();

        playerCharacter.transform.position = map.spawnPoint.transform.position;

        // �÷��̾� ���� (�÷��̾� ������ ���ε�)

        // UI �ε�

    }

    void LoadMap() 
    {
        map = Addressables.InstantiateAsync("SafeZone").WaitForCompletion().GetComponent<Map>();
    }

    void LoadPlayerCharacter() 
    {
        var handle = Addressables.InstantiateAsync("PlayerCharacter");
        // handle.Completed += (handle) => { playerCharacter = handle.Result.GetComponent<PlayerCharacter>(); };

        // SceneLoader.Instance.loadingCanvasController.SetProgressText("ĳ���� �ε� ��");

        handle.WaitForCompletion();
        // SceneLoader.Instance.loadingCanvasController.SetProgressText("ĳ���� �ε� �Ϸ�");

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
