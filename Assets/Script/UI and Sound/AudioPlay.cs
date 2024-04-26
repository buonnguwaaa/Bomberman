using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioPlay : MonoBehaviour
{
    [SerializeField] public GameObject settingPanel;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource runningSoundSource;
    public AudioClip mainMenu;
    public AudioClip background;
    public AudioClip pickItem;
    public AudioClip burn;
    public AudioClip death;
    public AudioClip passLevel;
    public AudioClip placeBomb;
    public AudioClip explose;
    public AudioClip gameOver;
    public AudioClip click;
    public AudioClip forestStep;
    public AudioClip levelUp;
    public AudioClip win;
    public AudioClip enemyDie;

    public static AudioPlay instance;
    void Awake()
	{
		if (instance)
		{
            gameObject.SetActive(false);
			Destroy(gameObject);
		}
		else
		{
			instance=this; 
            DontDestroyOnLoad(instance);
		}
	}

    public void Start() {
        musicSource.clip = mainMenu;
        musicSource.Play();
        runningSoundSource.clip = forestStep;
    }

    public void pauseMusicSource() {
        musicSource.Pause();
    }

    public void playMusicSource() {
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sound) {
        SFXSource.PlayOneShot(sound);
    }
    public void playRunningSoundSource(bool x) {
        runningSoundSource.enabled = x;
    }
    public void deathSound()
    {
        if(FindObjectOfType<GameManager>().lives>1) {
            musicSource.Stop();
            PlaySFX(death);
            musicSource.PlayDelayed(2.5f);
        }
        else {
            musicSource.Stop();
            PlaySFX(gameOver);
            playMainMenu(2f);
        }
    }
    public void burnSound()
    {
        PlaySFX(burn);
    }
    public void pickItemSound()
    {
        PlaySFX(pickItem);
    }
    public void passSound()
    {
        if (SceneManager.GetActiveScene().buildIndex == 5) {
            musicSource.Stop();
            PlaySFX(win);
            playMainMenu(3f);
        } else {
            musicSource.Stop();
            PlaySFX(passLevel);
            musicSource.PlayDelayed(1.25f);
        }

    }
    public void bombSound()
    {
        PlaySFX(placeBomb);
    }
    public void expolseSound()
    {
        PlaySFX(burn);
    }
    public void clickSound()
    {
        PlaySFX(click);
    }
    public void levelUpSound() {
        PlaySFX(levelUp);
    }
    public void enemyDieSound() {
        PlaySFX(enemyDie);
    }
    public void playBackgroundGame(float delay) {
        if (musicSource.clip == mainMenu) {
            musicSource.clip = background;
        }
        musicSource.PlayDelayed(delay);
    }
    public void playMainMenu(float delay) {
        if (musicSource.clip == background) {
            musicSource.clip = mainMenu;
        }
        musicSource.PlayDelayed(delay);
    }
}
