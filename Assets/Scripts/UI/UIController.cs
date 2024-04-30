using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public enum EUIShowMode 
{
    Single, Additive
}

public class UIController : MonoBehaviour
{
    private static UIController instance;
    public static UIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIController>();

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(UIController).Name);
                    instance = obj.AddComponent<UIController>();
                }
            }
            return instance;
        }
    }

    Stack<GameObject> uiStack = new Stack<GameObject>();
    Dictionary<string, GameObject> uiCacheDic = new Dictionary<string, GameObject>();
    PopUpUI popupUI;
    public EUIShowMode currentShowMode { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Push(string uiName, EUIShowMode eUIShowMode = EUIShowMode.Additive) 
    {
        if (!uiCacheDic.TryGetValue(uiName, out GameObject uiObject)) 
        { 
            uiObject = Addressables.InstantiateAsync(uiName).WaitForCompletion();
            if (uiObject == null) return;
            uiCacheDic.Add(uiName, uiObject);
            uiObject.transform.parent = this.transform;
        }
        
        if(eUIShowMode == EUIShowMode.Single) 
        {
            if(uiStack.TryPeek(out GameObject gameObject))
            {
                gameObject.SetActive(false);
            }
        }

        uiStack.Push(uiObject);
        uiObject.SetActive(true);
        uiObject.transform.parent = instance.transform;
        currentShowMode = eUIShowMode;
    }

    public bool Push<T>(string uiName, out T component, EUIShowMode eUIShowMode = EUIShowMode.Additive) where T : Component
    {
        component = null;

        if (!uiCacheDic.TryGetValue(uiName, out GameObject uiObject))
        {
            uiObject = Addressables.InstantiateAsync(uiName).WaitForCompletion();
            if (uiObject == null) return false;
            uiCacheDic.Add(uiName, uiObject);
            uiObject.transform.parent = this.transform;
        }

        if (eUIShowMode == EUIShowMode.Single)
        {
            if (uiStack.TryPeek(out GameObject gameObject))
            {
                gameObject.SetActive(false);
            }
        }

        uiStack.Push(uiObject);
        uiObject.SetActive(true);
        uiObject.transform.parent = instance.transform;

        component = uiObject.GetComponent<T>();
        currentShowMode = eUIShowMode;

        if (component) return true;
        return false;
    }


    public void Pop() 
    {
        if (uiStack.TryPeek(out GameObject go))
        {
            go.SetActive(false);
            Debug.Log("UIPOP!");
            uiStack.Pop();
        }

        if(uiStack.TryPeek(out GameObject next))
        {
            Debug.Log($"Next! : {next.name}");
            next.SetActive(true);
        }
    }

    public GameObject Peek() 
    {
        if(uiStack.TryPeek(out GameObject gameObject))
            return gameObject;
        return null;
    }

    public bool Peek(out GameObject gameObject) 
    {
        gameObject = null;

        if (uiStack.TryPeek(out GameObject obj)) 
        {
            gameObject = obj;
            return true;
        }
        return false;
    }

    public bool Peek<T>(out T component) where T : Component
    {
        component = null;

        if(Peek(out GameObject go)) 
        {
            component = go.GetComponent<T>();
            return true;
        }

        return false;
    }

    public void Clear() 
    {
        while(uiStack.Count > 0) 
        {
            Pop();
        }

        foreach(var i in uiCacheDic) 
        {
            Destroy(i.Value);
        }

        uiCacheDic.Clear();
    }

    public void ShowPopup(string msg, UnityAction onButtonNo, UnityAction onButtonYes) 
    {
        if(popupUI == null) 
        {
            popupUI = Addressables.InstantiateAsync("PopUpCanvas").WaitForCompletion().GetComponent<PopUpUI>();
            popupUI.transform.parent = this.transform;
        }

        popupUI.popupText.text = msg;
        popupUI.buttonNO.onClick.AddListener(onButtonNo);
        popupUI.buttonNO.onClick.AddListener(HidePopup);
        popupUI.buttonOK.onClick.AddListener(onButtonYes);
        popupUI.gameObject.SetActive(true);
    }

    void HidePopup() 
    {
        popupUI.buttonNO.onClick.RemoveAllListeners();
        popupUI.buttonOK.onClick.RemoveAllListeners();
        popupUI.gameObject.SetActive(false);
    }
}
