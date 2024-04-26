using System;
using System.Collections.Generic;
using System.Linq;
using TMPro; // Import namespace cho TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
	[Serializable]
	public struct ScoreData
	{
		public int score;
		public float timeElapsed;
	}

	[SerializeField] Transform entryContainer;
	[SerializeField] Transform entryTemplate;
	[SerializeField] Button[] buttons;
	int currentIndexButton = 0;

	// Danh sách dữ liệu điểm số
	List<ScoreData> scoreDataList;

	private void Awake()
	{
		entryTemplate.gameObject.SetActive(false);

		// Đọc dữ liệu từ file rankdata
		scoreDataList = ReadScoreDataFromFile("datarank.txt");

		// Hiển thị dữ liệu lên bảng xếp hạng
		DisplayScoreData();
	}

	private void Start()
	{
		buttons = FindObjectsOfType<Button>();
		buttons[currentIndexButton].Select();

		foreach (Button button in buttons)
		{
			button.onClick.AddListener(() => ButtonClicked(button));
		}
	}

	void ButtonClicked(Button clickedButton)
	{
		Debug.Log("Button clicked: " + clickedButton.name);
		AudioPlay.instance.clickSound();
		// if (clickedButton.name == "MenuBtn")
		// {
		// 	BackToStartGame();
		// }
		// else if (clickedButton.name == "BackBtn")
		// {
		// 	BackToEndgame();
		// }
	}

	void BackToEndgame()
	{
		SceneManager.LoadScene("Endgame");
	}

	void BackToStartGame()
	{
		FindObjectOfType<SettingVolume>().loadMusicVolume();
		FindObjectOfType<SettingVolume>().loadSFXVolume();
		SceneManager.LoadScene("StartMenu");
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	void DisplayScoreData()
	{
		Debug.Log("Number of entries: " + scoreDataList.Count);

		// Sắp xếp danh sách theo thứ tự giảm dần
		scoreDataList = scoreDataList.OrderByDescending(x => x.score).ThenBy(x => x.timeElapsed).ToList();

		float templateHeight = 45f;
		int maxEntries = 10;

		for (int i = 0; i < maxEntries; i++)
		{
			Transform entryTransform = Instantiate(entryTemplate, entryContainer);
			RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
			entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
			entryTransform.gameObject.SetActive(true);
			// Kiểm tra nếu i vượt quá số lượng dữ liệu thì hiển thị giá trị mặc định
			if (i < scoreDataList.Count)
			{
				int rank = i + 1;
				string rankString = GetRankString(rank);
				entryTransform.Find("PosText").GetComponent<TextMeshProUGUI>().text = rankString;

				// Gán dữ liệu vào TextMeshProUGUI Component
				entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = scoreDataList[i].score.ToString();
				entryTransform.Find("TimeText").GetComponent<TextMeshProUGUI>().text = FormatPlayTime(scoreDataList[i].timeElapsed);
			}
			else
			{
				// Hiển thị giá trị mặc định cho các mục còn lại
				entryTransform.Find("PosText").GetComponent<TextMeshProUGUI>().text = GetRankString(i + 1);
				entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = "<NULL>";
				entryTransform.Find("TimeText").GetComponent<TextMeshProUGUI>().text = "<NULL>";
			}
		}
	}

	List<ScoreData> ReadScoreDataFromFile(string filePath)
	{
		List<ScoreData> scoreDataList = new List<ScoreData>();

		// Kiểm tra xem file tồn tại hay không
		if (System.IO.File.Exists(filePath))
		{
			// Đọc từ file datarank.txt
			string[] lines = System.IO.File.ReadAllLines(filePath);

			foreach (string line in lines)
			{
				// Kiểm tra nếu dòng không rỗng
				if (!string.IsNullOrWhiteSpace(line))
				{
					// Sử dụng Regular Expression để trích xuất giá trị Score và TimeElapsed
					System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(line, @"Score:\s*(\d+),\s*TimeElapsed:\s*([\d.,]+)");

					if (match.Success)
					{
						ScoreData scoreData = new ScoreData();

						// Lấy giá trị từ nhóm capture trong Regular Expression
						scoreData.score = int.Parse(match.Groups[1].Value);

						// Sử dụng float.Parse để đọc giá trị timeElapsed
						scoreData.timeElapsed = float.Parse(match.Groups[2].Value.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);

						scoreDataList.Add(scoreData);
					}
				}
			}
		}

		return scoreDataList;
	}

	string GetRankString(int rank)
	{
		switch (rank)
		{
			case 1: return "1st";
			case 2: return "2nd";
			case 3: return "3rd";
			default: return rank + "th";
		}
	}

	string FormatPlayTime(float playTime)
	{
		playTime = (int)playTime;
		int minutes = Mathf.FloorToInt(playTime / 60);
		int seconds = Mathf.RoundToInt(playTime % 60);
		return string.Format("{0:00}:{1:00}", minutes, seconds);
	}
}
