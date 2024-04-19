using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonCompleteUI : MonoBehaviour
{
    public TMP_Text TotalKillCountText;
    public TMP_Text TotalDamageText;
    public TMP_Text TotalTakeDamageText;
    public TMP_Text TotalGoldText;
    public TMP_Text ClearTimeText;

    void OnEnable()
    {

    }

    void SetTexts() 
    {
        
    }

    public void OnClickRetry() 
    {
        Player.Instance.SaveData();
        SceneLoader.Instance.LoadScene(SceneLoader.Instance.loadSceneContext);
    }

    public void OnClickExit() 
    {
        Player.Instance.SaveData();
        SceneLoader.Instance.LoadScene(Defines.EScene.SafeZone);
    }

    public void OnCloseButton() 
    {
        UIController.Instance.Pop();
        Player.Instance.OnControllCharacter();
    }
}
