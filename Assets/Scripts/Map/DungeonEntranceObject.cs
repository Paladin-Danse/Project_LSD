using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntranceObject : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt()
    {
        return string.Format("[E] ���� â ����");
    }

    public void OnInteract()
    {
        //���� â Ʈ��
    }    
}
