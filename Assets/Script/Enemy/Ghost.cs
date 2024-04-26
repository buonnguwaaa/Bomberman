using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ghost : MonoBehaviour
{
    public Vector2 initPos;
    protected Rigidbody2D rb;
    public float speed;
    public int score;
    public int exp;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] protected Transform collisionCheck;
    [SerializeField] SpriteRenderer eyesSprite;
    [SerializeField] SpriteRenderer bodySprite;
    [SerializeField] Sprite[] eyesDir;
    [SerializeField] protected float distance;
    protected List<Vector2> dir;
    public Vector2 curDir;
    protected bool die=false;
    public virtual void Awake() {
        rb=GetComponent<Rigidbody2D>();
        initPos=transform.position;
    }
    public virtual void Start()
    {
        expText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        dir=new List<Vector2>();
        dir.Add(Vector2.left);
        dir.Add(Vector2.right);
        dir.Add(Vector2.down);
        dir.Add(Vector2.up);
        int idx= UnityEngine.Random.Range(0,dir.Count);
        eyesSprite.sprite=eyesDir[idx];
        curDir=dir[idx];
    }
    public virtual void Update()
    {
        Flip();
    }
    public virtual void FixedUpdate() {
        if(die||CollisionCheck()) return;
        Vector2 pos= rb.position;
        pos+=curDir*Time.fixedDeltaTime*speed;
        rb.MovePosition(pos);
    }
    public void Flip()
    {
        if(!CollisionCheck()) return;
        RoundPos();
        int newDir;
        do{
            newDir=Random.Range(0,dir.Count);
        }while(dir[newDir]==curDir);
        //||Physics2D.OverlapBox((Vector2)transform.position+dir[newDir],Vector2.one/2f,0,LayerMask.GetMask("Stage","Bomb")
        eyesSprite.sprite=eyesDir[newDir];
        curDir=dir[newDir];
    }
    public void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player"&&GameManager.instant.invulnerable==false)
        {
            other.GetComponent<PlayerMovement>().Die();
        }
    }
    public void Die()
    {
        // AudioPlay.instance.enemyDieSound();
        expText.SetText(exp.ToString());
        scoreText.SetText(score.ToString());
        expText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        GameManager.instant.AddScore(score);
        GameManager.instant.AddExp(exp);
        particleSystem.Play();
        rb.simulated=false;
        die=true;
        bodySprite.color=Color.black;
        Destroy(gameObject,1f);
    }
    public void RoundPos()
    {
       // Debug.Log(gameObject.name + ":(truoc)" + rb.position);
        Vector2Int round= new Vector2Int(Mathf.RoundToInt(rb.position.x),Mathf.RoundToInt(rb.position.y));
        rb.position=round;
       // Debug.Log(gameObject.name + ":(sau)" + rb.position);
    }
    public void OnDrawGizmos() {
        Vector3 pos= collisionCheck.position+(Vector3)curDir*distance;
        Gizmos.DrawLine(collisionCheck.position,pos);
    }
    public virtual bool CollisionCheck()
    {
       return Physics2D.Raycast(collisionCheck.position,curDir,distance,LayerMask.GetMask("Indestructible","Destructible","Bomb")); 
    }
    public void ResetPos()
    {
        transform.position=initPos;
    }
}
