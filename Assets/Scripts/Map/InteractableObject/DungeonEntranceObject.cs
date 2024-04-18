using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntranceObject : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt()
    {
        return string.Format("���� â ����");
    }

    public void OnInteract(Player player)
    {
        UIController.Instance.Push("DungeonSelectCanvas", EUIShowMode.Single);
        Player.Instance.OnControllUI();
    }
}
