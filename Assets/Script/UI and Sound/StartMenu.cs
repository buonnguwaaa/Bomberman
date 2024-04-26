using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    Button[] buttons;
    public GameObject rankPanel;
    public GameObject settingPanel;
    public GameObject buttonPanel;
    //0_rank; 1_quit; 2_play
    // void Awake()
    // {
    //     AudioPlay.instance.MainMenuSound();
    // }

    void Start()
    {
        // Find all buttons in the scene and assign them to the array
        buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => ButtonClicked(button));
        }
    }

    void ButtonClicked(Button clickedButton)
    {
        // Do something when a button is clicked
        Debug.Log("Button clicked: " + clickedButton.name);
        AudioPlay.instance.clickSound();
        if (clickedButton.name == "RankBtn")
        {
            RankTable();
        }
        else if(clickedButton.name == "PlayBtn")
        {
            playGame();
        }
        else if(clickedButton.name== "SettingBtn")
        {
            buttonPanel.SetActive(false);
            AudioPlay.instance.settingPanel.SetActive(true);
        }
        else if(clickedButton.name== "QuitBtn")
            quitGame();
    }

	public void RankTable()
	{
        rankPanel.SetActive(true);
        // SceneManager.LoadScene("RankMenu");
	}

	public void playGame() {
        AudioPlay.instance.playBackgroundGame(.75f);    
        SceneManager.LoadScene("Level1");
    }
    
    public void rankGame() {
        SceneManager.LoadScene("EndScene");
    }

    public void quitGame() {
        Application.Quit();
    }
    public void ButtonsOn()
    {
        buttonPanel.SetActive(true);
    }
}

