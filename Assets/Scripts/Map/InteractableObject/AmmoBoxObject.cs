using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxObject: MonoBehaviour, IInteractable
{
    public int supplyPercent = 20;
    public string GetInteractPrompt()
    {
        return string.Format("�Ѿ� ȹ��");
    }

    public void OnInteract(Player player)
    {
        ObjectPoolManager.Instance.TryPush(this.gameObject);
        Player.Instance.inventory.TakeAmmoItem(supplyPercent);
    }
}
