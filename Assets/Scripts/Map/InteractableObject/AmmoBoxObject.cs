using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxObject: MonoBehaviour, IInteractable
{
    public int ammoPercent;
    public string GetInteractPrompt()
    {
        return string.Format("�Ѿ� ȹ��");
    }

    public void OnInteract(Player player)
    {
        player.inventory.TakeAmmoItemColliderCrash(ammoPercent);
        ObjectPoolManager.Instance.TryPush(this.gameObject);
    }
}
