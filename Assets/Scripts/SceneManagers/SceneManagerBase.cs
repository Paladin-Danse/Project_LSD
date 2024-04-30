using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerBase : MonoBehaviour
{
    protected void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Pause() 
    { 
        Time.timeScale = 0f;
        Time.fixedDeltaTime = float.MaxValue;
    }

    public virtual void Resume() 
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    public virtual void OnLoadScene() 
    {
        
    }

    public virtual void OnUnloadScene() 
    {
        
    }
}

