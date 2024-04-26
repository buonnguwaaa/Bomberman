using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int exploseCount;
    Animator animator;
    private void Awake() {
        animator=GetComponentInChildren<Animator>();
    }
    void Start()
    {
        animator.SetInteger("Explose",exploseCount);
        Destroy(gameObject,0.6f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player"&&GameManager.instant.flamePass==false&&GameManager.instant.invulnerable==false)
        {
            PlayerMovement player=other.GetComponent<PlayerMovement>();
            player.Die(true);
        }
        else if(other.tag=="Ghost")
        {
            Ghost ghost=other.GetComponent<Ghost>();
            ghost.Die();
        }
         
    }
}
