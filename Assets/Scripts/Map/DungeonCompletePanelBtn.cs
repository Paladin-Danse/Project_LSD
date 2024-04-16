using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonCompletePanelBtn : MonoBehaviour
{
    public GameObject dungeonCompletePanel;
    DungeonMissionBoard missionBoard;
    public AudioSource retryAudioSource;
    public AudioSource ExitAudioSource;
    public AudioClip retryBtnSound;
    public AudioClip ExitBtnSound;
    WaitForSeconds WFS;

    private void Awake()
    {
        missionBoard = GetComponent<DungeonMissionBoard>();
        WFS = new WaitForSeconds(0.1f);
    }
    public void DungeonEntrance()
    {
        missionBoard.missionCompleteImage.SetActive(false);
        missionBoard.DungeonCompletePanel.SetActive(false);
        StartCoroutine(LoadScene());
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    IEnumerator LoadScene()
    {
        retryAudioSource.PlayOneShot(retryBtnSound);
        yield return WFS;
        yield return SceneManager.LoadSceneAsync("DungeonScene");
    }

    public void LoadSafeZone()
    {
        missionBoard.missionCompleteImage.SetActive(false);
        missionBoard.DungeonCompletePanel.SetActive(false);
        StartCoroutine(LoadSafeZoneScene());
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    IEnumerator LoadSafeZoneScene()
    {
        ExitAudioSource.PlayOneShot(ExitBtnSound);
        yield return WFS;
        yield return SceneManager.LoadSceneAsync("SafeZoneTestScene");
    }

    public void OnPanel()
    {
        dungeonCompletePanel.SetActive(true);
    }
    public void OffPanel()
    {
        dungeonCompletePanel.SetActive(false);
    }
}
