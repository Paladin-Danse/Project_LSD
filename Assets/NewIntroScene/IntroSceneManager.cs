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
    AudioSource audioSource;
    public AudioClip introStroySound;
    public GameObject stotyBackGround;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        skipButton.SetActive(false);
        nextButton.SetActive(false);
    }
    public void StoryAnimatorStart()
    {
        animator.SetTrigger("Play");
        audioSource.PlayOneShot(introStroySound);
        MainMenuButtons.SetActive(false);
        skipButton.SetActive(true);
        stotyBackGround.SetActive(true);
    }
    public void StoryAnimatorEnd() 
    {
        Debug.Log("스토리 애니메이션 종료");
        nextButton.SetActive(true);
        skipButton.SetActive(false);
        stotyBackGround.SetActive(false);
    }

    public void SkipButton()
    {
        animator.SetTrigger("Skip");
        audioSource.Stop();
    }

    public void GameStart()
    {
        Debug.Log("Game Start");
        // SceneManager.LoadScene("SafeZoneScene");
    }

    public void GameLoad()
    {
        Debug.Log("Load Game 미구현");
    }
    public void GameSetting()
    {
        Debug.Log("Setting 미구현");
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
