using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.AddressableAssets.GUI;
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
    Dictionary<AssetReference, GameObject> uiCacheDic = new Dictionary<AssetReference, GameObject>();
    PopUpUI popupUI;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Push(string uiName, EUIShowMode eUIShowMode = EUIShowMode.Additive) 
    {
        AssetReference assetReference = new AssetReference(uiName);
        if (! uiCacheDic.TryGetValue(assetReference, out GameObject uiObject)) 
        { 
            uiObject = Addressables.InstantiateAsync(assetReference).WaitForCompletion();
            uiCacheDic.Add(assetReference, uiObject);
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
    }

    public void Pop() 
    {
        if (uiStack.TryPeek(out GameObject gameObject))
        {
            gameObject.SetActive(false);
            Debug.Log("UIPOP!");
            uiStack.Pop();
        }

        if(uiStack.TryPeek(out GameObject next))
        {
            gameObject.SetActive(true);
        }
    }

    public GameObject Peek() 
    {
        if(uiStack.TryPeek(out GameObject gameObject))
            return gameObject;
        return null;
    }

    public void Clear() 
    {
        while(uiStack.Count > 0) 
        {
            Pop();
        }

        foreach(var i in uiCacheDic) 
        {
            i.Key.ReleaseInstance(i.Value);
            i.Key.ReleaseAsset();
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
