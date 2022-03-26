using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<float> OnHealthPctChange = delegate { };
    public event Action<float> OnStaminaPctChange = delegate { };
    public static event Action OnDeath = delegate { };

    public int maxHealth = 100;
    public int maxStamina = 100;
    [SerializeField] float invincileTime = 1f;

    public float currentHealth;
    public float currentStamina;
    public bool IsDead { get; private set; }
    public bool IsInvincible{ get; set; }
    public ParticleSystem particle;

    void OnEnable()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        GameManager.OnLevelUP += LevelUpBonus;
        
    }

    private void Start()
    {
        particle.Stop();
    }

    void LevelUpBonus(int level)
    {
        // 레벨에 따른 full 체력 스태미나 상승
        switch (level)
        {
            case 2:
                maxHealth = 150;
                maxStamina = 150;
                break;
            case 3:
                maxHealth = 200;
                maxStamina = 200;
                break;
            case 4:
                maxHealth = 250;
                maxStamina = 250;
                break;
            case 5:
                maxHealth = 300;
                maxStamina = 300;
                break;
        }
        ShowLvUpEffect();
    }

    private void ShowLvUpEffect()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        OnHealthPctChange(100);
        OnStaminaPctChange(100);
        particle.Play();
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            DeathBehavior();
        }

        //NaturalHeal();
    }

    private void NaturalHeal()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1 *Time.deltaTime*0.5f;
        }
        return;
    }

    void DeathBehavior()
    {
        if (IsDead) { return; }

        GetComponent<Animator>().SetTrigger("Die");

        IsDead = true;
        OnDeath(); 
    }

    public void SubtractHealth(int damage)
    {
        if (IsInvincible) return;
        currentHealth -= damage;
        float percentage = currentHealth / maxHealth;
        OnHealthPctChange(percentage);
    }

    public void SubtractStamina(int stamina)
    {
        currentStamina -= stamina;
        float percentage = currentStamina / maxStamina;
        OnStaminaPctChange(percentage);
    }

    public void HealHealth(int damage)
    {
        currentHealth += damage;
        float percentage = currentHealth / maxHealth;
        OnHealthPctChange(percentage);
    }

    public void HealStamina(int stamina)
    {
        currentStamina += stamina;
        float percentage = currentStamina / maxStamina;
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

    private void OnDisable()
    {
        GameManager.OnLevelUP -= LevelUpBonus;
    }


}
