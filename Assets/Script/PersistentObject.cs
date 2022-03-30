using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    [SerializeField] GameObject[] objects;
    
    // static�� �ؾ� �� �ٸ� �ν��Ͻ����� �ش� bool�� ���� ����
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
