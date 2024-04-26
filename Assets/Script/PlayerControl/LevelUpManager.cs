using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpManager : MonoBehaviour
{
    [SerializeField] Image abilityImg;
	[SerializeField] TextMeshProUGUI abilityInfo;
	[SerializeField] Sprite[] abilitiesImg;
	[SerializeField] string[] abilitiesInfo;
	[SerializeField] GameObject levelUpContainer;
    void Start()
    {
        levelUpContainer.SetActive(false);
    }

    void Update()
    {
        if(levelUpContainer.active==true&&Input.anyKey)
		{
			Time.timeScale=1;
			levelUpContainer.SetActive(false);
			AudioPlay.instance.playMusicSource();
		}
    }
    public void LevelUp(int curLevel)
    {
		
        if(curLevel==2)
		{
			Stats.instance.blockPassOn();			
			GameManager.instant.softBlockPass=true;
		} 
		if(curLevel==3) {
			GameManager.instant.flamePass=true;
			Stats.instance.flamePassOn();
		}
		if(curLevel==4) {
			GameManager.instant.kickBomb=true;
			Stats.instance.bombKickOn();
		}
		if(curLevel==5) {
			GameManager.instant.remoteControl=true;
			Stats.instance.remoteControlOn();
		}
		abilityImg.sprite=abilitiesImg[curLevel-2];
		abilityInfo.SetText(abilitiesInfo[curLevel-2]);
		levelUpContainer.SetActive(true);
		AudioPlay.instance.pauseMusicSource();
		AudioPlay.instance.levelUpSound();
		Time.timeScale=0;
    }
}
