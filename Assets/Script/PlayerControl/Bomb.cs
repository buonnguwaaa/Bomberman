using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    BombController bombController;
    ExploseContainer exploseContainer;
    Rigidbody2D rb;
    public float timeToExplose=3f;
    public int speed;
    Vector2 curDir;
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        exploseContainer=FindObjectOfType<ExploseContainer>();
        bombController=FindObjectOfType<BombController>();
    }
    void Start()
    {
        curDir=new Vector2(0,0);
        rb.bodyType=RigidbodyType2D.Static;
    }
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.LeftShift)&&GameManager.instant.remoteControl)
        {
            timeToExplose=0;
        }
        timeToExplose-=Time.deltaTime;
        if(timeToExplose<=0)
        {
            explose();
        }
        if(CollisionCheck()&&rb.velocity!=Vector2.zero)
        {
            RoundPos();
            rb.velocity=new Vector2(0,0);
        }
    }

    void explose()
    {
        RoundPos();
        AudioPlay.instance.expolseSound();
        exploseContainer.Explosion(rb.position);
        if(bombController.bombRemain<5) bombController.bombRemain++;
        Destroy(gameObject);
    }
    public void KickedAway(Vector2 dir)
    {
        rb.bodyType=RigidbodyType2D.Dynamic;
        rb.simulated=true;
        rb.AddForce(dir*speed);
        curDir=dir;
    }
    void OnCollisionEnter2D()
    {
        RoundPos();
        rb.velocity=new Vector2(0,0);
    }
    public bool CollisionCheck()
    {
       return Physics2D.Raycast(transform.position,curDir,1,LayerMask.GetMask("Indestructible","Destructible","Ghost")); 
    }
    void RoundPos()
    {
        rb.position=new Vector2(Mathf.RoundToInt(rb.position.x),Mathf.RoundToInt(rb.position.y));
    }
}
