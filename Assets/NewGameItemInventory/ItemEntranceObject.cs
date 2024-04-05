using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntranceObject : MonoBehaviour, IInteractable
{
    public ItemData item;

    public string GetInteractPrompt()
    {
        return string.Format("Pickup {0}", item.displayName);
    }

    public void OnInteract()
    {
        //Inventory.instance.AddItem(item); // 인벤토리(싱클톤화)에 아이템 추가하기
        Destroy(gameObject); // 아이템 제거
    }
}