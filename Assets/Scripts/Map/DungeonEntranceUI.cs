using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DungeonEntranceUI : MonoBehaviour
{
    public static DungeonEntranceUI instance;    

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        Cursor.visible = true;
        
    }
    void Update()
    {
        transform.position = Input.mousePosition;
        //if(EventSystem.current.IsPointerOverGameObject() == true)
        //{
        //    GreenLine.SetActive(true);
        //}
        //else if(EventSystem.current.IsPointerOverGameObject() == false)
        //{
        //    GreenLine.SetActive(false);
        //}
    }

    public void OnGreenLine(GameObject GreenLine)
    {
        GreenLine.SetActive(true);
    }

    public void OffGreenLine(GameObject GreenLine)
    {
        GreenLine.SetActive(false);
    }
}
