using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgSound; // 배경음
    public AudioClip[] bgList; // 배경음리스트

    public AudioMixer audioMixer;
    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // 씬시작시 실행되는 메서드
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bgList.Length; i++)
        {
            if (arg0.name == bgList[i].name)
            {
                BgSoundPlay(bgList[i]); break; 
            }
        }
    }

    

    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        bgSound.Play();
    }
}
