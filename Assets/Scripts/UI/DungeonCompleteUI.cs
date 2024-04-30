using System;
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
        TotalKillCountText.text = DungeonTracker.Instance.killedEnemies.ToString();
        TotalDamageText.text = DungeonTracker.Instance.totalDamage.ToString();
        TotalTakeDamageText.text = DungeonTracker.Instance.receivedDamage.ToString();
        TotalGoldText.text = DungeonTracker.Instance.earnGold.ToString();

        TimeSpan t = TimeSpan.FromSeconds(DungeonTracker.Instance.missionTime);
        ClearTimeText.text = string.Format("{0:D1}:{1:D2}", t.Minutes, t.Seconds);
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
