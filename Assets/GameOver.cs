using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject PlayerUI;
    public GameObject PauseMenuButton;
    public GameObject GameOverUI;


    public RoomFader sceneFader;


    public void Retry()
    {
        sceneFader.FadeToScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        PlayerUI.SetActive(!PlayerUI.activeSelf);
        PauseMenuButton.SetActive(PauseMenuButton.activeSelf);
        GameOverUI.SetActive(GameOverUI.activeSelf);
    }

    public void GoGameOver()
    {
        PlayerUI.SetActive(!PlayerUI.activeSelf);
        PauseMenuButton.SetActive(false);
        GameOverUI.SetActive(true);
    }

    public void MainMenu()
    {
        sceneFader.FadeToScene(0);
    }
}
