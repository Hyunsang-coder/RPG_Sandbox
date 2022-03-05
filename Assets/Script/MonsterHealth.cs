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
    
    // Enable�� ���ÿ� hp 100 ä���, �ｺ �� �ν��Ͻ�ȭ 
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
        OnHealthRemoved(this); // �ｺ �� ���ֱ�

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

