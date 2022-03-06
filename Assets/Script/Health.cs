using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    public event Action<float> OnHealthPctChange = delegate { };

    [SerializeField] float maxHealth = 100;
    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    void OnEnable()
    {
        CurrentHealth = maxHealth;
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

        GetComponent<Animator>().SetTrigger("Die");

        IsDead = true;
    }

    public void SubtractHealth(int damage)
    {
        CurrentHealth -= damage;
        float percentage = CurrentHealth / maxHealth;
        OnHealthPctChange(percentage);
    }

}
