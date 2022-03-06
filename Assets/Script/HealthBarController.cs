using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    

    //Key값인 MonsterHealth에 따라 value인 인스턴스를 추가/삭제 
    Dictionary<MonsterHealth, MonsterHealthBar> healthBars = new Dictionary<MonsterHealth, MonsterHealthBar>();
    [SerializeField] MonsterHealthBar healthBarPrefab;

    // 헬스 바 추가/제거 로직 등록 
    private void Awake()
    {
        MonsterHealth.OnHealthAdded += AddHealthBar;
        MonsterHealth.OnHealthRemoved += RemoveHealthBar;
    }
    
    // 헬스 바 인스턴스화 
    void AddHealthBar(MonsterHealth monsterHealth)
    {
        // Dictionary에 MonsterHealth가 없으면, 헬스바 인스턴스
        if (!healthBars.ContainsKey(monsterHealth))
        {
            var healthBar = Instantiate(healthBarPrefab, transform);
            healthBars.Add(monsterHealth, healthBar);
            healthBar.SetMonsterHealth(monsterHealth);
        }
    }

    // 헬스 바 파괴
    void RemoveHealthBar(MonsterHealth monsterHealth)
    {
        // Dictionary에 MonsterHealth가 있으면, 해당 오브젝트 파괴
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
