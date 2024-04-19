using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDungeonKeep : MonoBehaviour
{
    private static SelectedDungeonKeep _instance;
    public static SelectedDungeonKeep instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gameObject = GameObject.FindObjectOfType<SelectedDungeonKeep>().gameObject;
                if (gameObject == null)
                {
                    gameObject = new GameObject("SelectedDungeon");
                    _instance = gameObject.AddComponent<SelectedDungeonKeep>();
                }
            }
            return _instance;
        }
    }
    public int mapNumber;
}
