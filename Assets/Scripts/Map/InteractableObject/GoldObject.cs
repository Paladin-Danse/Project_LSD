using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldObject : MonoBehaviour, IInteractable
{
    int gold;

    private void Awake()
    {
        gold = Random.Range(10, 200);        
    }

    public string GetInteractPrompt()
    {
        return "<b>[F]</b> µ· ÁÝ±â";
    }

    public void OnInteract(Player player)
    {
        player.inventory.money += gold;
        DungeonTracker.Instance.earnGold += gold;
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
