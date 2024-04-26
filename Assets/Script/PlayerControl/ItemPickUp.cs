using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public enum Item{
        blastRadius,
        extraBomb,
        extraSpeed,
        extraLife,
        key,
        score300,
        score400,
        score500,
        score600,
        score700,
        score900,
        score1000,
    }
    public Item item;

    void Start()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player")
        {
            if(item==Item.blastRadius)
            {
                if(GameManager.instant.exploseRadius<5) 
                {
                    GameManager.instant.exploseRadius++;
                    Stats.instance.changeRadius(GameManager.instant.exploseRadius);  
                }
            }
            if(item==Item.extraBomb)
            {
                if(GameManager.instant.bombAmount<5)
                {
                    other.GetComponent<BombController>().bombRemain++;
                    GameManager.instant.bombAmount++;
                    Stats.instance.changeBomb(GameManager.instant.bombAmount);
                }
            }
            if(item==Item.extraSpeed)
            {
                if(GameManager.instant.speed<8)
                {
                    GameManager.instant.speed+=2;
                    Stats.instance.changeSpeed((int)GameManager.instant.speed);
                } 
            }
            if(item==Item.extraLife)
            {
                GameManager.instant.AddLives();
            }
            if(item==Item.score300)
            {
                GameManager.instant.AddScore(300);
            }
            if(item==Item.score400)
            {
                GameManager.instant.AddScore(400);
            }
            if(item==Item.score500)
            {
                GameManager.instant.AddScore(500);
            }
            if(item==Item.score600)
            {
                GameManager.instant.AddScore(600);
            }
            if(item==Item.score700)
            {
                GameManager.instant.AddScore(700);
            }
            if(item==Item.score900)
            {
                GameManager.instant.AddScore(900);
            }
            if(item==Item.score1000)
            {
                GameManager.instant.AddScore(1000);
            }
            if(item==Item.key)
            {
                FindObjectOfType<ExitDoor>().key=true;
                Debug.Log("hasKey");
            }
            AudioPlay.instance.pickItemSound();
            Destroy(gameObject);
        }
    }
}
