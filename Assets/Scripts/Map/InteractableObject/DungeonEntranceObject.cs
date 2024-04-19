using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntranceObject : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt()
    {
        return string.Format("던전 창 오픈");
    }

    public void OnInteract(Player player)
    {
        UIController.Instance.Push("DungeonSelectCanvas", EUIShowMode.Single);
        Player.Instance.OnControllUI();
    }
}
