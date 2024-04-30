using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    private Animator animator;
    public GameObject skipButton;
    public GameObject nextButton;
    public GameObject mainMenuButtons;
    public GameObject settingMenu;
    public GameObject title;
    public GameObject storyBackground;
    
    private void Awake()
    {            
        animator = GetComponent<Animator>();
        SceneLoader.Instance.LoadCompleted();
    }

    void Start()
    {        
        skipButton.SetActive(false);
        nextButton.SetActive(false);
    }
    public void StoryAnimatorStart()
    {
        settingMenu.SetActive(false);
        title.SetActive(false);
        storyBackground.SetActive(true);
        animator.SetTrigger("Play");
        SoundManager.instance.storySoundSource.volume = 0.5f;
        SoundManager.instance.storySoundSource.PlayOneShot(SoundManager.instance.storySound);        
        mainMenuButtons.SetActive(false);
        skipButton.SetActive(true);
    }
    public void StoryAnimatorEnd() 
    {
        Debug.Log("스토리 애니메이션 종료");
        nextButton.SetActive(true);
        skipButton.SetActive(false);
        SceneLoader.Instance.LoadScene(Defines.EScene.SafeZone);
    }

    public void SkipButton()
    {
        animator.SetTrigger("Skip");
        SoundManager.instance.storySoundSource.Stop();        
        SceneLoader.Instance.LoadScene(Defines.EScene.SafeZone);
    }

    public void GameStart()
    {
        Debug.Log("Game Start");
        SceneManager.LoadScene("SafeZoneScene");
    }

    public void GameLoad()
    {
        Debug.Log("Load Game 미구현");
    }

    [System.Obsolete]
    public void GameSetting()
    {
        settingMenu.SetActive(!settingMenu.active);
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
