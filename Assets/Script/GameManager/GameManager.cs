using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	[Header("Player Info")]
	public bool canPlaceBomb=true;
	public float speed=4;
	public int lives = 3;
	public int score = 0;
	public int exploseRadius=1;
	public int bombAmount=1;
	public int exp=0;
	public int playerLevel=1;
	public Dictionary<int, int> expEachLevel;
	public float rateHasItem=0.15f;
	public bool kickBomb;
	public bool flamePass;
	public bool softBlockPass;
	public bool remoteControl;
	public bool invulnerable;
	[Header("UI")]
	[SerializeField] Slider expSlider;
	[SerializeField] TextMeshProUGUI timerText;
	[SerializeField] TextMeshProUGUI scoreText;

	public static GameManager instant;
	[SerializeField] GameObject key;
	public int monster = -1;
	[SerializeField] float timeToLoad;
	Timer timer;
	bool create;
	[SerializeField] Vector2[] posOfKey;
	// Đối tượng SaveData để lưu trữ dữ liệu
	private SaveData saveData;
	
	void Awake()
	{
		// Kiểm tra xem có đối tượng SaveData đã được tạo chưa
		saveData = FindObjectOfType<SaveData>();
		if (saveData == null)
		{
			// Nếu chưa có, tạo một đối tượng mới
			GameObject saveDataObject = new GameObject("SaveData");
			saveData = saveDataObject.AddComponent<SaveData>();
		}

		// Cài đặt SaveData để không bị hủy khi chuyển scene
		DontDestroyOnLoad(saveData.gameObject);
		if (instant)
		{
			instant.create=false;
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
		else
		{
			instant=this;
			expEachLevel = new Dictionary<int, int>();
			expEachLevel[1]=500;
			expEachLevel[2]=800;
			expEachLevel[3]=1100;
			expEachLevel[4]=1500;
			expEachLevel[5]=1;
			expSlider.value=(float)exp/expEachLevel[playerLevel];
			DontDestroyOnLoad(gameObject);
		}
	}

	void Start()
	{
		
		scoreText.SetText("Score: "+score.ToString("00000"));
		Stats.instance.changeHeart(lives);
		timer = gameObject.AddComponent<Timer>();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.RightShift))
		{
			HackGame();
		}
		monster = GameObject.FindGameObjectsWithTag("Ghost").Length;
		if (monster == 0 && !create)
		{
			Instantiate(key, posOfKey[SceneManager.GetActiveScene().buildIndex - 1], Quaternion.identity);
			create = true;

		}
		UpdateTimer();
	}

	void UpdateTimer()
	{
		timerText.text = "Time:" +timer.GetFormattedTime();
	}

	public void AddScore(int score)
	{
		this.score += score;
		scoreText.SetText("Score:" +this.score.ToString("00000"));
	}

	public void AddLives()
	{
		this.lives++;
		Stats.instance.changeHeart(lives);
	}
	public void AddExp(int n)
	{
		this.exp+=n;
		if(playerLevel<5&&exp>=expEachLevel[playerLevel])
		{
			exp=0;
			playerLevel++;
			bool have=false;
			int random;
			do
			{
				have=false;
				random=Random.Range(2,6);
				if(random==2&&softBlockPass)
				{
					have=true;
				}
				if(random==3&&flamePass)
				{
					have=true;
				}
				if(random==4&&kickBomb)
				{
					have=true;
				}
				if(random==5&&remoteControl)
				{
					have=true;
				}
			}while(have);
			gameObject.GetComponent<LevelUpManager>().LevelUp(random);
		}
		expSlider.value=(float)exp/expEachLevel[playerLevel];
		if(playerLevel==5) expSlider.value=1;	
	}
	public void ResetPlayerInfo() 
	{
		instant.playerLevel=1;
		instant.exp=0;
		instant.speed=4;
		instant.bombAmount=1;
		instant.exploseRadius=1;
		expSlider.value=0;
		softBlockPass=false;
		flamePass=false;
		kickBomb=false;
		remoteControl=false;
		Stats.instance.Reset();
	}
	public void HackGame()
	{
		instant.playerLevel=5;
		instant.exp=1;
		instant.speed=8;
		FindObjectOfType<BombController>().bombRemain=5;
		instant.bombAmount=5;
		instant.exploseRadius=5;
		expSlider.value=1;
		softBlockPass=true;
		flamePass=true;
		kickBomb=true;
		remoteControl=true;
		invulnerable=true;
		Stats.instance.AllOn();
	}
    public void Die()
    {
        this.lives--;
        if(lives==0)
        {
			//Hàm gọi khi hết mạng
            SaveDataToFile();
			LoadGameOver();
			return;
        }
		instant.ResetPlayerInfo();
        int idx=SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadScene(idx));
    }
	
    public IEnumerator LoadScene(int idx)
    {
        yield return new WaitForSeconds(timeToLoad);
        SceneManager.LoadScene(idx);
        GameObject[] ghosts=GameObject.FindGameObjectsWithTag("Ghost");
        foreach(GameObject ghost in ghosts)
        {
            ghost.GetComponent<Ghost>().ResetPos();
        }
    }
    public IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(timeToLoad);
        FindObjectOfType<ScenePresist>().ResetPersist();
        int curScene=SceneManager.GetActiveScene().buildIndex;
        int nextScene=curScene+1;
        if(nextScene==6)
        {
			SaveDataToFile();
            LoadEndGame();
        }
        FindObjectOfType<ScenePresist>().ResetPersist();
        SceneManager.LoadScene(nextScene);
    }

	public void LoadMenuScene()
	{
		FindObjectOfType<ScenePresist>().ResetPersist();
		Destroy(gameObject);
		SceneManager.LoadScene("StartMenu");
	}

	// Hàm load khi win
	public void LoadEndGame()
	{
		FindObjectOfType<ScenePresist>().ResetPersist();
		Destroy(gameObject);
		SceneManager.LoadScene("EndGame");
	}

	public void LoadGameOver()
	{
		FindObjectOfType<ScenePresist>().ResetPersist();
		Destroy(gameObject);
		SceneManager.LoadScene("GameOver");
	}

	// Lưu dữ liệu vào file
	void SaveDataToFile()
	{
		// Gán dữ liệu từ GameManager vào SaveData
		saveData.Score = this.score;
		saveData.TimeElapsed = this.timer.GetElapsedTime();

		// Lưu dữ liệu vào file
		saveData.SaveDataToFile();
	}
}

public class Timer : MonoBehaviour
{
	float elapsedTime;

	// Lấy thời gian đã trôi qua
	public float GetElapsedTime()
	{
		return elapsedTime;
	}

	// Lấy thời gian đã trôi qua dưới dạng chuỗi định dạng
	public string GetFormattedTime()
	{
		int minutes = Mathf.FloorToInt(elapsedTime / 60);
		int seconds = Mathf.FloorToInt(elapsedTime % 60);
		return string.Format("{0:00}:{1:00}", minutes, seconds);
	}

	// Update is called once per frame
	void Update()
	{
		elapsedTime += Time.deltaTime;
	}
	public void LoadMenuScene()
	{
		Destroy(gameObject);
		SceneManager.LoadScene("StartMenu");
	}
}

public class SaveData : MonoBehaviour
{
	private static SaveData instance;

	public int Score { get; set; }
	public float TimeElapsed { get; set; }

	private string saveFilePath = "SaveData.txt";

	void Awake()
	{
		// Đảm bảo chỉ có một instance tồn tại
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}

		LoadData();
	}

	// Các phương thức khác như trước

	public void SaveDataToFile()
	{
		// Kiểm tra xem file đã tồn tại chưa
		if (!File.Exists(saveFilePath))
		{
			// Nếu chưa tồn tại, tạo mới file
			File.Create(saveFilePath).Dispose();
		}

		// Mở hoặc tạo file để ghi với quyền sử dụng được chia sẻ
		using (FileStream fileStream = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
		using (StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8))
		{
			// Ghi dữ liệu vào file
			writer.WriteLine($"TimeElapsed: {TimeElapsed}");
			writer.WriteLine($"Score: {Score}");
		}
	}


	private void LoadData()
	{
		// Kiểm tra xem file tồn tại hay không
		if (File.Exists(saveFilePath))
		{
			// Mở file để đọc dữ liệu
			using (StreamReader reader = new StreamReader(saveFilePath))
			{
				// Đọc từng dòng dữ liệu và cập nhật biến tương ứng
				string line = reader.ReadLine();
				while (line != null)
				{
					if (line.StartsWith("Score"))
					{
						Score = int.Parse(line.Split(':')[1].Trim());
					}
					else if (line.StartsWith("TimeElapsed"))
					{
						TimeElapsed = float.Parse(line.Split(':')[1].Trim());
					}

					line = reader.ReadLine();
				}
			}
		}
	}
}