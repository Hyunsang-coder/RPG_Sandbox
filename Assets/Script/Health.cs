using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    public event Action<float> OnHealthPctChange = delegate { };
    public event Action<float> OnStaminaPctChange = delegate { };
    public static event Action OnDeath = delegate { };

    [SerializeField] float maxHealth = 100;
    [SerializeField] float maxStamina = 100;
    [SerializeField] float invincileTime = 1f;
    public float CurrentHealth { get; private set; }
    public float CurrentStamina { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsInvincible{ get; set; }

    void OnEnable()
    {
        CurrentHealth = maxHealth;
        CurrentStamina = maxStamina;
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
        OnDeath(); 
        Debug.Log(OnDeath);
    }

    public void SubtractHealth(int damage)
    {
        if (IsInvincible) return;
        CurrentHealth -= damage;
        float percentage = CurrentHealth / maxHealth;
        OnHealthPctChange(percentage);
    }

    public void SubtractStamina(int stamina)
    {
        CurrentStamina -= stamina;
        float percentage = CurrentStamina / maxStamina;
        OnStaminaPctChange(percentage);
    }

    public void Invincible()
    {
        StartCoroutine(InvincibleWindow());
    }

    IEnumerator InvincibleWindow()
    {
        IsInvincible = true;
        Debug.Log("invincible!");
        yield return new WaitForSeconds(invincileTime);
        
        IsInvincible = false;
        Debug.Log("normal!");
        yield return null;
    }

    
}
