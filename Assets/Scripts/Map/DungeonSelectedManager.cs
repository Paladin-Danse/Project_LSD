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
        if(SelectedDungeon[dungeonNum].Ddata.QuestID != 0)
            dungeonEntrancePanel.SetActive(true);
        else
            dungeonEntrancePanel.SetActive(false);
        SelectedDungeonContext.Instance.selectedDungeonData = SelectedDungeon[dungeonNum].Ddata;
    }

    public void OffDungeonEntrancePanel()
    {
        dungeonEntrancePanel.SetActive(false);
    }    

    public void DungeonEntrance()
    {
        UIController.Instance.Pop();
        SceneLoader.Instance.LoadScene(Defines.EScene.Dungeon);

    }
    public void CloseDungeonSelectUI() 
    {
        UIController.Instance.Pop();
    }
}
