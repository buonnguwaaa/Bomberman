using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingVolume : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    public void Start() {
        if (PlayerPrefs.HasKey("musicVolume")) {
            loadMusicVolume();
        }
            setMusicVolume();
        if (PlayerPrefs.HasKey("SFXVolume")) {
            loadSFXVolume();
        }
            setSFXVolume();
    }

    
    //Setting volume
    public void setMusicVolume() {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void setSFXVolume() {
        float volume = SFXSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void loadMusicVolume() {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    public void loadSFXVolume() {
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void Back()
    {
        FindObjectOfType<StartMenu>().ButtonsOn();
        AudioPlay.instance.settingPanel.SetActive(false);
    }
}
