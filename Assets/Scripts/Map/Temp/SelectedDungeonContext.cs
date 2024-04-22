using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDungeonContext : MonoBehaviour
{
    private static SelectedDungeonContext instance;
    public static SelectedDungeonContext Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SelectedDungeonContext>();

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(SelectedDungeonContext).Name);
                    instance = obj.AddComponent<SelectedDungeonContext>();
                }
            }
            return instance;
        }
    }

    public DungeonData selectedDungeonData;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
