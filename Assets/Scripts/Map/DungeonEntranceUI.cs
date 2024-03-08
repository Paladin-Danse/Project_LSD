using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DungeonEntranceUI : MonoBehaviour
{
    public static DungeonEntranceUI instance;
    public GameObject GreenLine;

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
        GreenLine.SetActive(false);
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

    public void OnGreenLine()
    {
        GreenLine.SetActive(true);
    }

    public void OffGreenLine()
    {
        GreenLine.SetActive(false);
    }
}
