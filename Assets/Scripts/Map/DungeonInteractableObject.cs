using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInteractableObject : MonoBehaviour, IInteractable
{    
    public string GetInteractPrompt()
    {
        return string.Format("�Ѿ� ȹ��");
    }

    public void OnInteract()
    {
        Destroy(gameObject);
    }
}
