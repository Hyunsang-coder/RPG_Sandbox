using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    [SerializeField] GameObject[] objects;
    
    // static을 해야 또 다른 인스턴스에서 해당 bool에 접근 가능
    static bool hasSpawned = false;
    void Awake()
    {
        if (hasSpawned) return;


        SpawnPersistentObject();
        

        hasSpawned = true;
    }

    void SpawnPersistentObject()
    {
        foreach (var obj in objects)
        {
            GameObject persistentObject = Instantiate(obj);
            DontDestroyOnLoad(persistentObject);
        }
    }

    
}
