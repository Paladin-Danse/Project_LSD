using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallShipObject : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt()
    {
        return string.Format("던전 결과창 열기");
    }

    public void OnInteract(Player player)
    {
        UIController.Instance.Push("DungeonCompleteUI", EUIShowMode.Single);
        Player.Instance.OnControllUI();
    }
}
