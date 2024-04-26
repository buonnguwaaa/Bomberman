using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EndGame : MonoBehaviour
{
	Button[] buttons;
	int currentIndexButton = 2;
	[SerializeField] TextMeshProUGUI Score;
	[SerializeField] TextMeshProUGUI Time;
	public GameObject rankPanel;
	public int score;
	public float time;

	// Reference đến ReadData

	/// </summary>
	// 0_rank; 1_quit; 2_play	
	void Start()
	{
		// Find all buttons in the scene and assign them to the array
		ReadDataFromFile();
		WriteDataToRankFile(score, time);
		UpdateUI();
		buttons = FindObjectsOfType<Button>();

		// Example: Add a listener to each button's onClick event
		foreach (Button button in buttons)
		{
			button.onClick.AddListener(() => ButtonClicked(button));
		}
	}

	void ButtonClicked(Button clickedButton)
	{
		AudioPlay.instance.clickSound();
		// Do something when a button is clicked
		Debug.Log("Button clicked: " + clickedButton.name);

		// Check if the clicked button is the "Rank" button
		if (clickedButton.name == "RankBtn")
		{
			rankGame();
		}
		else if (clickedButton.name == "PlayBtn")
		{
			playGame();
		} else if (clickedButton.name == "MenuBtn") {
			mainMenu();
		}
		else
			quitGame();
	}

	public void playGame()
	{
		AudioPlay.instance.playBackgroundGame(.75f);   
		SceneManager.LoadScene("Level1");
	}

	public void rankGame()
	{
		rankPanel.SetActive(true);
		// SceneManager.LoadScene("EndScene");
	}
	public void mainMenu() {
		FindObjectOfType<SettingVolume>().loadMusicVolume();
        FindObjectOfType<SettingVolume>().loadSFXVolume();
        SceneManager.LoadScene("StartMenu");
        AudioPlay.instance.playMainMenu(.5f);
	}
	public void quitGame()
	{
		Application.Quit();
	}
	void ReadDataFromFile()
	{
		string saveFilePath = "SaveData.txt";

		if (File.Exists(saveFilePath))
		{
			using (StreamReader reader = new StreamReader(saveFilePath))
			{
				string line;

				while ((line = reader.ReadLine()) != null)
				{
					if (line.StartsWith("Score"))
					{
						score = int.Parse(line.Split(':')[1].Trim());
					}
					else if (line.StartsWith("TimeElapsed"))
					{
						time = float.Parse(line.Split(':')[1].Trim());
					}
				}
			}
		}
	}
	void UpdateUI()
	{
		Score.SetText(score.ToString());
		Time.SetText(FormatTime(time));
	}

	string FormatTime(float seconds)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
		return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
	}
	void WriteDataToRankFile(int score, float time)
	{
		string rankFilePath = "datarank.txt";

		// Kiểm tra xem dữ liệu đã tồn tại trong file hay chưa
		if (!IsDataExistsInRankFile(score, time, rankFilePath))
		{
			// Ghi vào file "datarank.txt"
			using (StreamWriter writer = new StreamWriter(rankFilePath, true))
			{
				writer.WriteLine($"Score: {score}, TimeElapsed: {time}");
			}
		}
	}

	bool IsDataExistsInRankFile(int score, float time, string filePath)
	{
		if (File.Exists(filePath))
		{
			using (StreamReader reader = new StreamReader(filePath))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					// Kiểm tra từng dòng để xem có dữ liệu giống nhau không
					if (line.Contains($"Score: {score}, TimeElapsed: {time}"))
					{
						return true; // Dữ liệu đã tồn tại
					}
				}
			}
		}

		return false; // Dữ liệu chưa tồn tại
	}
}

