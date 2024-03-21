using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidKitObject : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt()
    {
        return string.Format("ü�� ȸ��");
    }

    public void OnInteract()
    {
        Player.FindObjectOfType<Health>().curHealth += 30;
        Destroy(gameObject);
    }
}
