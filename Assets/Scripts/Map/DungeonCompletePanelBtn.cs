using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonCompletePanelBtn : MonoBehaviour
{
    public GameObject dungeonCompletePanel;
    DungeonMissionBoard missionBoard;

    private void Awake()
    {
        missionBoard = GetComponent<DungeonMissionBoard>();
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
