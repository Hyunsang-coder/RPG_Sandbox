using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth: MonoBehaviour
{
    public event Action<float> OnHealthChange = delegate { };

    [SerializeField] float maxHP = 100;

    public bool isDead = false;
    float currentHealth;

    
    void OnEnable()
    {
        currentHealth = maxHP;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            EnemyDie();
        }
    }

    void EnemyDie()
    {
        if (isDead) { return; }

        if (this.gameObject.tag == "Dragon")
        {
            GetComponent<NavMeshAgent>().baseOffset = -0.6f;
        }
        GetComponent<NavMeshAgent>().isStopped = true;
        GetComponent<Animator>().SetTrigger("Die");

        Destroy(gameObject, 5f);

        isDead = true;
    }

    
    public void SubtractHealth(int damage)
    {
        currentHealth -= damage;
        float healthPercent = currentHealth / maxHP;

        OnHealthChange(healthPercent);
    }
}
