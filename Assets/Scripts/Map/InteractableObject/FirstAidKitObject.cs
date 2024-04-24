using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidKitObject : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt()
    {
        return string.Format("체력 회복");
    }

    public void OnInteract(Player player)
    {
        Player.FindObjectOfType<Health>().curHealth += 30;
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
