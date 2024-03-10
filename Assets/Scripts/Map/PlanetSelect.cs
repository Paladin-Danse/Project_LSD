using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlanetSelect : MonoBehaviour
{
    public GameObject GreenLine;    

    public void MouseOverUI()
    {
        GreenLine.SetActive(true);
    }

    public void MouseExitUI()
    {
        GreenLine.SetActive(false);
    }
}
