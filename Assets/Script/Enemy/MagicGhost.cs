using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicGhost : Ghost
{
    [SerializeField] public float coolDown=1f;
    [SerializeField] public float timer;
    [SerializeField] public GameObject fireBall;
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
        base.Update();
        if(timer<0)
        {
            Vector3 rotate;
            if(curDir==Vector2.up) rotate=new Vector3(0,0,90);
            else if(curDir ==Vector2.left) rotate=new Vector3(0,0,180);
            else if(curDir==Vector2.down) rotate=new Vector3(0,0,270);
            else rotate=new Vector3(0,0,0);
          //  Debug.Log(rotate);
            GameObject instant=Instantiate(fireBall,transform.position,Quaternion.Euler(rotate),transform);
            instant.GetComponent<FireBall>().Move(curDir);
            timer=coolDown;
        }
        timer-=Time.deltaTime;
    }
}
