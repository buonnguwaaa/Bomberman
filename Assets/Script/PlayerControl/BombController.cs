using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombController : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    
    public int bombRemain;
    GameManager gameManager;
    private void Awake() {
        
        gameManager=FindObjectOfType<GameManager>();
    }
    void Start()
    {
        bombRemain=GameManager.instant.bombAmount;
    }
    // Update is called once per frame
    void Update()
    {   
        if(gameObject.GetComponent<PlayerMovement>().die||!GameManager.instant.canPlaceBomb) return;
        if(Input.GetKeyDown(KeyCode.Space)&&!gameObject.GetComponent<CapsuleCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ghost")))
        {
            if(bombRemain>=1) PlaceBomb();
        }
    }

    void PlaceBomb()
    {
        AudioPlay.instance.bombSound();
        bombRemain--;
        Vector2 bomPos;
        bomPos.x = Mathf.RoundToInt(transform.position.x);
        bomPos.y = Mathf.RoundToInt(transform.position.y);
        Instantiate(bomb, bomPos, Quaternion.identity);
    }
    void OnTriggerExit2D(Collider2D other) {
        if(other.tag=="Bomb")
        {
            other.isTrigger=false;
        }
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag=="Bomb"&&GameManager.instant.kickBomb)
        {
            other.gameObject.GetComponent<Bomb>().KickedAway(new Vector2(PlayerMovement.horizontal,PlayerMovement.vertical));
        }
    }
}
