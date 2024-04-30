using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxObject: MonoBehaviour, IInteractable
{
    public int supplyPercent = 20;
    public string GetInteractPrompt()
    {
        return string.Format("Ammo 획득");
    }

    public void OnInteract(Player player)
    {
        player.inventory.TakeAmmoItem(supplyPercent);
        ObjectPoolManager.Instance.TryPush(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.TryGetComponent<PlayerCharacter>(out PlayerCharacter playerCharacter))
            {
                OnInteract(playerCharacter.ownedPlayer);
            }
        }
    }
}
