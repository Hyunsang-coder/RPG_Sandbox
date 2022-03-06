using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    

    //Key���� MonsterHealth�� ���� value�� �ν��Ͻ��� �߰�/���� 
    Dictionary<MonsterHealth, MonsterHealthBar> healthBars = new Dictionary<MonsterHealth, MonsterHealthBar>();
    [SerializeField] MonsterHealthBar healthBarPrefab;

    // �ｺ �� �߰�/���� ���� ��� 
    private void Awake()
    {
        MonsterHealth.OnHealthAdded += AddHealthBar;
        MonsterHealth.OnHealthRemoved += RemoveHealthBar;
    }
    
    // �ｺ �� �ν��Ͻ�ȭ 
    void AddHealthBar(MonsterHealth monsterHealth)
    {
        // Dictionary�� MonsterHealth�� ������, �ｺ�� �ν��Ͻ�
        if (!healthBars.ContainsKey(monsterHealth))
        {
            var healthBar = Instantiate(healthBarPrefab, transform);
            healthBars.Add(monsterHealth, healthBar);
            healthBar.SetMonsterHealth(monsterHealth);
        }
    }

    // �ｺ �� �ı�
    void RemoveHealthBar(MonsterHealth monsterHealth)
    {
        // Dictionary�� MonsterHealth�� ������, �ش� ������Ʈ �ı�
        if(healthBars.ContainsKey(monsterHealth))
        {
            Destroy(healthBars[monsterHealth].gameObject);
            healthBars.Remove(monsterHealth);
        }
        else
        {
            return;
        }

    }
}
