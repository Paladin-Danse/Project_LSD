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
    private static DungeonSelectedManager _instance;
    public static DungeonSelectedManager instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject gameObject = GameObject.FindObjectOfType<DungeonSelectedManager>().gameObject;
                if (gameObject == null)
                {
                    gameObject = new GameObject("DungeonSelectedManager");
                    _instance = gameObject.AddComponent<DungeonSelectedManager>();
                }
            }
            return _instance;
        }
    }
    public GameObject dungeonEntrancePanel;
    public TextMeshProUGUI dungeonNameOfPanel;
    public Dungeon[] SelectedDungeon; 
    public RectTransform tooltipRectTransform;
    public RectTransform backgroundRectTransform;

    //public Action LoadSceneEvent;            
    private void Update()
    {
        Vector2 tooltipPosition = Input.mousePosition;        
        tooltipRectTransform.position = tooltipPosition;
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

    public void DungeonEntrance()
    {
        //LoadSceneEvent();
        StartCoroutine(LoadScene());        
    }

    IEnumerator LoadScene()
    {
        yield return SceneManager.LoadSceneAsync("DungeonScene");
    }
}
