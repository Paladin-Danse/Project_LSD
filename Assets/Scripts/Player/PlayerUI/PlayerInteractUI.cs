using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerInteractUI : MonoBehaviour, IPlayerUIInterface
{
    public TMP_Text promptText;

    public void BindUI(PlayerCharacter character)
    {
        character.ownedPlayer.playerInteract.OnInteractableChanged += SetPromptText;
        RefreshUI();
    }

    public void RefreshUI()
    {
        SetPromptText(string.Empty);
    }

    public void UnbindUI(PlayerCharacter character)
    {
        character.ownedPlayer.playerInteract.OnInteractableChanged -= SetPromptText;
    }

    void SetPromptText(string text) 
    {
        if(text != string.Empty) 
        {
            promptText.text = $"<b>[F]</b> : {text}";
        }
    }
}
