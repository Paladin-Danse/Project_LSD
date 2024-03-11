using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DungeonSelect : MonoBehaviour
{
    public GameObject GreenLine;    
    public GameObject dungeonEntrancePanel;
    public TextMeshProUGUI dungeonNameOfPanel;
    public DungeonData[] SelectedDungeon;
    public GameObject dungeonTooltip;

    public void MouseOverUI()
    {
        GreenLine.SetActive(true);
    }

    public void MouseExitUI()
    {
        GreenLine.SetActive(false);
    }

    public void OnDungeonEntrancePanel(int dungeonNum)
    {
        dungeonNameOfPanel.text = SelectedDungeon[dungeonNum].dungeonName;
        dungeonEntrancePanel.SetActive(true);
    }

    public void OffDungeonEntrancePanel()
    {
        dungeonEntrancePanel.SetActive(false);
    }

    public void ShowTooltip()
    {
        dungeonTooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        dungeonTooltip.SetActive(false);
    }
}
