using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
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
        Player.Instance.UnPossess();
        UIController.Instance.Clear();
        CutSceneManager.Instance.playableDirector.Play();        
        Invoke("LoadScene", 12f);
    }

    void LoadScene()
    {        
        SceneLoader.Instance.LoadScene(Defines.EScene.Dungeon);
    }

    public void CloseDungeonSelectUI() 
    {
        UIController.Instance.Pop();
    }
}
