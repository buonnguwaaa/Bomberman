using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Stats : MonoBehaviour
{
    public static Stats instance;
    [SerializeField] TextMeshProUGUI heart;
    [SerializeField] TextMeshProUGUI bomb;
    [SerializeField] TextMeshProUGUI radius;
    [SerializeField] TextMeshProUGUI speed;
    [SerializeField] Image blockPass;
    [SerializeField] Image flamePass;
    [SerializeField] Image remoteControl;
    [SerializeField] Image bombKick;
    [SerializeField] Color offColor;
    [SerializeField] Color onColor;
    
    void Awake() 
    {
        if(!instance) instance=this;
    }
    void Start()
    {
        Reset();
    }
    public void changeHeart(int cnt)
    {
        heart.SetText(cnt.ToString());
    } 
    public void changeBomb(int cnt)
    {
        bomb.SetText(cnt.ToString());
    }
    public void changeRadius(int cnt)
    {
        radius.SetText(cnt.ToString());
    }
    public void changeSpeed(int cnt)
    {
        speed.SetText(cnt.ToString());
    }

    public void flamePassOn()
    {
        flamePass.color= onColor;
    }
    public void flamePassOff()
    {
        flamePass.color= offColor;
    }

    public void blockPassOn()
    {
        blockPass.color= onColor;
    }
    public void blockPassOff()
    {
        blockPass.color= offColor;
    }

    public void bombKickOn()
    {
        bombKick.color= onColor;
    }
    public void bombKickOff()
    {
        bombKick.color= offColor;
    }

    public void remoteControlOn()
    {
        remoteControl.color= onColor;
    }
    public void remoteControlOff()
    {
        remoteControl.color= offColor;
    }
    public void Reset()
    {
        instance.blockPassOff();
        instance.flamePassOff();
        instance.bombKickOff();
        instance.remoteControlOff();
        instance.changeHeart(GameManager.instant.lives);
		instance.changeBomb(GameManager.instant.bombAmount);
		instance.changeRadius(GameManager.instant.exploseRadius);
		instance.changeSpeed((int)GameManager.instant.speed);
    }
    public void AllOn()
    {
        instance.blockPassOn();
        instance.flamePassOn();
        instance.bombKickOn();
        instance.remoteControlOn();
        instance.changeHeart(GameManager.instant.lives);
		instance.changeBomb(GameManager.instant.bombAmount);
		instance.changeRadius(GameManager.instant.exploseRadius);
		instance.changeSpeed((int)GameManager.instant.speed);
    }


}
