using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlanetSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject GreenLine;

    private void Start()
    {
        GreenLine.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GreenLine.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GreenLine.SetActive(false);
    }
}
