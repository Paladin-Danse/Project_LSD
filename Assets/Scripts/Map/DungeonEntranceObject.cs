using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntranceObject : MonoBehaviour, IInteractable
{
    public GameObject dungeonSelectedUI;
    public string GetInteractPrompt()
    {
        return string.Format("[E] 던전 창 오픈");
    }

    public void OnInteract()
    {
        dungeonSelectedUI.SetActive(true);
    }    
}
