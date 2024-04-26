using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePresist : MonoBehaviour
{
    void Awake()
    {
        int numberSessions=FindObjectsOfType<ScenePresist>().Length;
        if(numberSessions>1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetPersist()
    {
        Destroy(gameObject);
    }
}
