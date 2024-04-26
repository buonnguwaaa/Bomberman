using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public int isPause = 0;
    public GameObject pauseMenu;
    public GameObject settingUI;
    public Button[] buttons;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPause == 0) {
                pauseGame();
            } else {
                resumeGame();
            }
        }
    }
    void Start()
    {
        pauseMenu.SetActive(false);
    }
    public void pauseGame() {
        AudioPlay.instance.pauseMusicSource();
        pauseMenu.SetActive(true);
        Time.timeScale=0;
        isPause = 1;
    }

    public void resumeGame() {
        pauseMenu.SetActive(false);
        AudioPlay.instance.playMusicSource();
        Time.timeScale=1;
        isPause = 0;
    }

    public void settingGame() {
        settingUI.SetActive(true);
    }

    public void mainMenu() {
        isPause = 0;
        Time.timeScale=1;
        FindObjectOfType<SettingVolume>().loadMusicVolume();
        FindObjectOfType<SettingVolume>().loadSFXVolume();
        FindObjectOfType<GameManager>().LoadMenuScene();
        AudioPlay.instance.playMainMenu(.5f);
    }

    public void quitGame() {
        Application.Quit();
    }

    public void clickSound() {
        AudioPlay.instance.clickSound();
    }
}
