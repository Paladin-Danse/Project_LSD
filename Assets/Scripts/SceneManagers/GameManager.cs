using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(GameManager).Name);
                    instance = obj.AddComponent<GameManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    public void PauseGame() 
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = float.MaxValue;
    }

    public void ResumeGame() 
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
