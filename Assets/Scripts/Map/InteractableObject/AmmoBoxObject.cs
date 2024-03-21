using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxObject: MonoBehaviour, IInteractable
{    
    public string GetInteractPrompt()
    {
        return string.Format("ÃÑ¾Ë È¹µæ");
    }

    public void OnInteract()
    {
        Destroy(gameObject);
    }
}
