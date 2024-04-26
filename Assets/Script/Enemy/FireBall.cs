using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] float speed;
    void Start()
    {
        Destroy(gameObject,10f);
    }
    public void Move(Vector2 dir)
    {
        GetComponent<Rigidbody2D>().AddForce(dir*speed);
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player"&&GameManager.instant.invulnerable==false)
        {
            PlayerMovement player=other.GetComponent<PlayerMovement>();
            player.Die(true);
        }
        Destroy(gameObject);
    }
}
