using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public interface IInteractable
{
    string GetInteractPrompt();

    void OnInteract(Player player);
}

public class PlayerInteract : MonoBehaviour
{
    private IInteractable curInteractable;

    public Action<string> OnInteractableChanged;

    Player player;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<IInteractable>(out IInteractable nowInteractable))
        {
            if (nowInteractable != curInteractable)
            {
                curInteractable = nowInteractable;
                OnInteractableChanged?.Invoke(curInteractable.GetInteractPrompt());
            }
            return;
        }
        else 
        {
            curInteractable = null;
            OnInteractableChanged?.Invoke(String.Empty);
        }
    }

    public void OnInteractInput(InputAction.CallbackContext callbackContext)
    {
        if(curInteractable != null)
        {
            curInteractable.OnInteract(Player.Instance);
            curInteractable = null; 
        }        
    }

    public void RegisterPlayer(Player player) 
    {
        this.player = player;
    }
}
