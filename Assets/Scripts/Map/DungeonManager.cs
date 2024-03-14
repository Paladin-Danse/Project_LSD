using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
    public RectTransform tooltipRectTransform;
    public RectTransform backgroundRectTransform;

    int entranceNumber;    

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
        Vector2 tooltipPosition = Input.mousePosition;        
        tooltipRectTransform.position = tooltipPosition;
    }    

    public void OnDungeonEntrancePanel(int dungeonNum)
    {
        dungeonNameOfPanel.text = SelectedDungeon[dungeonNum].Ddata.dungeonName;
        dungeonEntrancePanel.SetActive(true);
        entranceNumber = dungeonNum;
    }

    public void OffDungeonEntrancePanel()
    {
        dungeonEntrancePanel.SetActive(false);
    }    

    public void DungeonEntrance()
    {
        StartCoroutine(LoadScene());        
    }

    IEnumerator LoadScene()
    {
        if (entranceNumber == 0)
        {
            yield return SceneManager.LoadSceneAsync("DungeonScene");
        }
        else if (entranceNumber == 1)
        {
            yield return null;
        }        
    }
}
