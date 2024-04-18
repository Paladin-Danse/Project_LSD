using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Dungeon
{
    public DungeonData Ddata;
}
public class DungeonSelectedManager : MonoBehaviour
{
    public GameObject dungeonEntrancePanel;
    public TextMeshProUGUI dungeonNameOfPanel;
    public Dungeon[] SelectedDungeon; 
    public RectTransform tooltipRectTransform;
    public RectTransform backgroundRectTransform;

    private void Update()
    {
        Vector2 tooltipPosition = Input.mousePosition;        
        tooltipRectTransform.position = tooltipPosition;
    }    

    public void OnDungeonEntrancePanel(int dungeonNum)
    {
        dungeonNameOfPanel.text = SelectedDungeon[dungeonNum].Ddata.dungeonName;
        dungeonEntrancePanel.SetActive(true);
        SelectedDungeonContext.Instance.mapNumber = dungeonNum;
    }

    public void OffDungeonEntrancePanel()
    {
        dungeonEntrancePanel.SetActive(false);
    }    

    public void DungeonEntrance()
    {
        UIController.Instance.Pop();
        // todo : 던전으로 변경 필요
        SceneLoader.Instance.LoadScene(Defines.EScene.SafeZone);
    }

    IEnumerator LoadScene()
    {
        yield return SceneManager.LoadSceneAsync("DungeonScene");
    }

    public void CloseDungeonSelectUI() 
    {
        UIController.Instance.Pop();
    }
}
