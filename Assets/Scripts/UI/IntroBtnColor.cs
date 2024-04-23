using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroBtnColor : MonoBehaviour
{    
    Button Btn;

    private void Awake()
    {
        Btn = GetComponent<Button>();
    }

    public void ChangeBtnColor()
    {
        Btn.GetComponent<Image>().color = new Color32(255, 140, 0, 255);
    }

    public void ResetBtnColor()
    {
        Btn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }    
}
