using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveStone : MonoBehaviour
{
    [SerializeField] GameObject ghost;
    public int amount=4;
    public float coolDown=5f;
    public float timer=0;
    void Awake()
    {
        timer=coolDown;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer-=Time.deltaTime;
        if(timer<0&&amount>0)
        {
            amount--;
            Instantiate(ghost,transform.position,Quaternion.identity,transform);
            timer=coolDown;
        }
    }
}
