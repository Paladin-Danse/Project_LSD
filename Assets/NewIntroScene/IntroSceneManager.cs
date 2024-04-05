using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    private Animator animator;
    public GameObject skipButton;
    public GameObject nextButton;
    public GameObject MainMenuButtons;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        skipButton.SetActive(false);
        nextButton.SetActive(false);
    }
    public void StoryAnimatorStart()
    {
        animator.SetTrigger("Play");
        MainMenuButtons.SetActive(false);
        skipButton.SetActive(true);
    }
    public void StoryAnimatorEnd() 
    {
        Debug.Log("���丮 �ִϸ��̼� ����");
        nextButton.SetActive(true);
        skipButton.SetActive(false);
    }

    public void SkipButton()
    {
        animator.SetTrigger("Skip");
    }

    public void GameStart()
    {
        Debug.Log("Game Start");
        // SceneManager.LoadScene("SafeZoneScene");
    }

    public void GameLoad()
    {
        Debug.Log("Load Game �̱���");
    }
    public void GameSetting()
    {
        Debug.Log("Setting �̱���");
    }

    public void GameExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
