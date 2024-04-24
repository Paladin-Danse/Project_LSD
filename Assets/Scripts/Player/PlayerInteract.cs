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
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance = 1f;
    public LayerMask layerMask;
    private IInteractable curInteractable;

    private Camera camera { get { return Camera.main; } }

    public Action<string> OnInteractableChanged;

    Player player;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Interactable");
    }

    void Start()
    {
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.TryGetComponent<IInteractable>(out IInteractable nowInteractable))
                {
                    if(nowInteractable != curInteractable) 
                    {
                        curInteractable = nowInteractable;
                        OnInteractableChanged?.Invoke(curInteractable.GetInteractPrompt());
                    }
                    return;
                }
            }
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
