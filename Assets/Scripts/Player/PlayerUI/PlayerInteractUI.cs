using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerInteractUI : MonoBehaviour, IPlayerUIInterface
{
    public TMP_Text promptText;

    public PlayerCharacter playerCharacter { get; set; }

    public void BindUI(PlayerCharacter character)
    {
        playerCharacter = character;
        playerCharacter.ownedPlayer.playerInteract.OnInteractableChanged += SetPromptText;
        RefreshUI();
    }

    public void RefreshUI()
    {
        SetPromptText(string.Empty);
    }

    public void UnbindUI()
    {
        playerCharacter.ownedPlayer.playerInteract.OnInteractableChanged -= SetPromptText;
    }

    void SetPromptText(string text) 
    {
        if(text != string.Empty) 
        {
            promptText.gameObject.SetActive(true);
            promptText.text = $"<b>[F]</b> : {text}";
        }
        else 
        {
            promptText.gameObject.SetActive(false);
        }
    }
}
