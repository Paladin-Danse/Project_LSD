using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntranceObject : MonoBehaviour, IInteractable
{
    public GameObject dungeonSelectedUI;
    public bool isDungeonSelectedUI = false;
    public string GetInteractPrompt()
    {
        return string.Format("´øÀü Ã¢ ¿ÀÇÂ");
    }

    public void OnInteract()
    {
        dungeonSelectedUI.SetActive(true);
        isDungeonSelectedUI = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void OffDungeonSelectedUI()
    {
        dungeonSelectedUI.SetActive(false);
        isDungeonSelectedUI = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
