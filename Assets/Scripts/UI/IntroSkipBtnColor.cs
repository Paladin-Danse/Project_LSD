using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSkipBtnColor : MonoBehaviour
{
    public GameObject skipBtnColor;

    public void OnColor()
    {
        skipBtnColor.SetActive(true);
    }

    public void OffColor()
    {
        skipBtnColor.SetActive(false);
    }
}
