using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntranceObject : MonoBehaviour, IInteractable
{
    public GameObject dungeonSelectedUI;
    public string GetInteractPrompt()
    {
        return string.Format("���� â ����");
    }

    public void OnInteract()
    {
        dungeonSelectedUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }    
}
