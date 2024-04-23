using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxObject: MonoBehaviour, IInteractable
{
    public int supplyPercent = 20;
    public string GetInteractPrompt()
    {
        return string.Format("ÃÑ¾Ë È¹µæ");
    }

    public void OnInteract(Player player)
    {
        player.inventory.TakeAmmoItem(supplyPercent);
        ObjectPoolManager.Instance.TryPush(this.gameObject);
    }
}
