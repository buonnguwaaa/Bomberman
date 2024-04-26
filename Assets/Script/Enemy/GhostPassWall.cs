using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPassWall : MagicGhost
{
    public override void Awake()
    {
        base.Awake();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        Flip();
        if(timer<0)
        {
            Vector2 pos= new Vector2(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.y));
           // Debug.Log(pos);
            foreach(Vector2 curDir in dir)
            {
                Vector3 rotate;
                if(curDir==Vector2.up) rotate=new Vector3(0,0,90);
                else if(curDir ==Vector2.left) rotate=new Vector3(0,0,180);
                else if(curDir==Vector2.down) rotate=new Vector3(0,0,270);
                else rotate=new Vector3(0,0,0);
                GameObject instant=Instantiate(fireBall,pos,Quaternion.Euler(rotate),transform);
                instant.GetComponent<FireBall>().Move(curDir);
            }
            timer=coolDown;
        }
        timer-=Time.deltaTime;
    }

    public override bool CollisionCheck()
    {
       return Physics2D.Raycast(collisionCheck.position,curDir,distance,LayerMask.GetMask("Bomb","Indestructible")); 
    }
}
