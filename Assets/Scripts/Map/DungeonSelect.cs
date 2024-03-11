using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSelect : MonoBehaviour
{
    public GameObject GreenLine;
    public GameObject dungeonTooltipPanel;
    public DungeonData dungeonData;
    

    DungeonTooltip dungeonTooltip;

    private void Awake()
    {
        dungeonTooltip = dungeonTooltipPanel.GetComponent<DungeonTooltip>();
    }

    public void MouseOverUI()
    {
        GreenLine.SetActive(true);
        dungeonTooltip.OnTooltip(DungeonManager.instance.SelectedDungeon[dungeonData.dungeonNumber]);
        dungeonTooltipPanel.SetActive(true);
    }

    public void MouseExitUI()
    {
        GreenLine.SetActive(false);
        dungeonTooltipPanel.SetActive(false);
    }
}
