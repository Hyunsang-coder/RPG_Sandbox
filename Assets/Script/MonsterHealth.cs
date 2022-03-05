using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterHealth : MonoBehaviour
{
    public static Action<MonsterHealth> OnHealthAdded = delegate { };
    public static Action<MonsterHealth> OnHealthRemoved = delegate { };
    public event Action<float> OnHealthPctChange = delegate { };


    [SerializeField] float maxHealth = 100;
    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }
    
    // Enable과 동시에 hp 100 채우고, 헬스 바 인스턴스화 
    void OnEnable()
    {
        CurrentHealth = maxHealth;
        OnHealthAdded(this);
    }

    void Update()
    {
        if (CurrentHealth <= 0)
        {
            DeathBehavior();
        }
    }

    void DeathBehavior()
    {
        if (IsDead) { return; }

        GetComponent<NavMeshAgent>().isStopped = true;
        GetComponent<Animator>().SetTrigger("Die");

        Destroy(gameObject, 2f);
        OnHealthRemoved(this); // 헬스 바 없애기

        IsDead = true;
    }


    public void SubtractHealth(int damage)
    {
        CurrentHealth -= damage;
        float pct = CurrentHealth / maxHealth;
        OnHealthPctChange(pct);
        Debug.Log(CurrentHealth);
    }



}

