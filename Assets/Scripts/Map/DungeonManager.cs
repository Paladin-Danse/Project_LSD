using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Dungeon
{
    public DungeonData Ddata;
}
public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;
    public GameObject dungeonEntrancePanel;
    public TextMeshProUGUI dungeonNameOfPanel;
    public Dungeon[] SelectedDungeon;    
    public RectTransform rectTransform;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }
    private void Update()
    {
        rectTransform.position = Input.mousePosition;        
    }    

    public void OnDungeonEntrancePanel(int dungeonNum)
    {
        dungeonNameOfPanel.text = SelectedDungeon[dungeonNum].Ddata.dungeonName;
        dungeonEntrancePanel.SetActive(true);
    }

    public void OffDungeonEntrancePanel()
    {
        dungeonEntrancePanel.SetActive(false);
    }    
}
