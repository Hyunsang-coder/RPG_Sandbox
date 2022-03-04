using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;
    bool isDead;
    
    // delegate ���� = �� ��ũ��Ʈ�� publisher, ���⼭�� Action ���� ����
    public event Action<float> OnHealthPercentChange = delegate { };

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            DeathBehavior();
        }
    }

    void DeathBehavior()
    {
        if (isDead)
        {
            return;
        }

        if (this.gameObject.tag == "Enemy")
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<Animator>().SetTrigger("Dead");

            Destroy(gameObject, 2f);
        }
        isDead = true;
    }

    public void SubtractHealth(int damage)
    {
        currentHealth -= damage;
        float currentHealthPct = currentHealth / maxHealth;
        OnHealthPercentChange(currentHealthPct);    
    }

}
