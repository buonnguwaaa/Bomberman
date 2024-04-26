using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public static float horizontal;
    public static float vertical;
    Animator animator;
    Rigidbody2D rb;
    public Vector2 curDir;
    public float distance=0.7f;
    [SerializeField] ParticleSystem particleSystem;
    
    public bool die;

    private void Awake() {
        animator=GetComponentInChildren<Animator>();
        rb=GetComponent<Rigidbody2D>();
        
    }
    void Start()
    {
        die=false;
        animator.SetFloat("dirX",0);
        animator.SetFloat("dirY",-1);
    }

    // Update is called once per frame
    void Update()
    {
        if(die) return;
        if(CollisionCheck()) GameManager.instant.canPlaceBomb=false;
        else GameManager.instant.canPlaceBomb=true;
        horizontal=Input.GetAxisRaw("Horizontal");
        vertical=Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() {
        if(die) 
        {
            AudioPlay.instance.playRunningSoundSource(false);
            return;
        }
        if(horizontal!=0||vertical!=0)
        {
            AudioPlay.instance.playRunningSoundSource(true);
            curDir=new Vector2(horizontal,vertical);
            animator.SetBool("Move",true);
            animator.SetFloat("dirX",horizontal);
            animator.SetFloat("dirY",vertical);
            Vector2 pos= rb.position;
            pos+=new Vector2(horizontal,vertical)*Time.fixedDeltaTime*GameManager.instant.speed;
            rb.MovePosition(pos);
        }   
        else
        {
            AudioPlay.instance.playRunningSoundSource(false);
            animator.SetBool("Move",false);
        }
    }
    public void Die(bool burn=false)
    {
       
        die=true;
        rb.simulated=false;
        animator.SetBool("Die",die);
        if(burn)
        {
            AudioPlay.instance.burnSound();
            gameObject.GetComponentInChildren<SpriteRenderer>().color=new Color(0.5754717f,0.5754717f,0.5754717f,1f);
            particleSystem.Play();
        }
        AudioPlay.instance.deathSound();
        Invoke("Disappear",1.25f);
    }
    void Disappear()
    {
        FindObjectOfType<GameManager>().Die();
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag=="Destructible"&&GameManager.instant.softBlockPass)
        {
            other.gameObject.GetComponent<TilemapCollider2D>().isTrigger=true;
            
        }
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag=="Destructible")
        {
            other.gameObject.GetComponent<TilemapCollider2D>().isTrigger=false;
        }
    }

    public void OnDrawGizmos() 
    {
        Vector3 pos= transform.position+(Vector3)curDir*distance;
        Gizmos.DrawLine(transform.position,pos);
    }
    public bool CollisionCheck()
    {
       return Physics2D.Raycast(transform.position,curDir,distance,LayerMask.GetMask("Destructible","Indestructible")); 
    }


    // public void OnTriggerEnter2D(Collider2D other) {
    //     if(other.tag=="Ghost")
    //     {
    //         gameObject.GetComponent<BombController>().enabled=false;
    //         return;
    //     }
    // }
}
