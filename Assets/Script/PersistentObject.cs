using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    //PersistentObject[] persistentObjects;
    public static PersistentObject instance;
    void Awake()
    {
        
        //persistentObjects = FindObjectsOfType<PersistentObject>();
        //if (persistentObjects.Length > 1)
        
        if (instance != null)
        {
            Destroy(gameObject);
        } 
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    
}
