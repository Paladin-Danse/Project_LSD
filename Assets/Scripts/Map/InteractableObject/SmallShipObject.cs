using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallShipObject : MonoBehaviour, IInteractable
{
    public GameObject dungeonCompletePanel;

    public string GetInteractPrompt()
    {
        return string.Format("던전 결과창 열기");
    }

    public void OnInteract(Player player)
    {
        DungeonManager.Instance.missionBoard.DungeonCompletePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        //ObjectPoolManager.Instance.TryPush(this.gameObject);
    }
}
