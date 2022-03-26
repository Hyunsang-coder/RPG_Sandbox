using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth: MonoBehaviour
{
    public event Action<float> OnHealthChange = delegate { };

    [SerializeField] float maxHP = 100;
    [SerializeField] int enemyXP = 100;

    public bool isDead = false;
    float currentHealth;

    NavMeshAgent navMeshAgent;
    Animator animator;
    GameManager gameManager;
    
    void OnEnable()
    {
        currentHealth = maxHP;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
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
        gameManager.GainExperience(enemyXP);

        if (this.gameObject.tag == "Dragon")
        {
            navMeshAgent.baseOffset = -0.6f;
        }
        navMeshAgent.isStopped = true;
        animator.SetTrigger("Die");

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
