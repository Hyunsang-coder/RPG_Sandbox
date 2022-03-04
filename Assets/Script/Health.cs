using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    
    [SerializeField] float maxHealth = 100;
    public float CurrentHealth { get; private set; }
    bool isDead;
    
    // delegate ���� = �� ��ũ��Ʈ�� publisher, ���⼭�� Action ���� ����
    public event Action<float> OnHealthPercentChange = delegate { };
    public event Action OnPlayerDeath = delegate { };

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
        if (isDead){return;}

        if (this.gameObject.tag == "Enemy")
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<Animator>().SetTrigger("Dead");

            Destroy(gameObject, 2f);
        }

        if (this.gameObject.tag == "Player")
        {
            GetComponent<Animator>().SetTrigger("Die");
        }

        OnPlayerDeath();
        isDead = true;
    }

    public void SubtractHealth(int damage)
    {
        CurrentHealth -= damage;
        float currentHealthPct = CurrentHealth / maxHealth;
        OnHealthPercentChange(currentHealthPct);    
    }

    public bool IsDead
    {
        get { return isDead; }
    }

}
