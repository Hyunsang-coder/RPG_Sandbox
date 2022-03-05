using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image healthImage;
    [SerializeField] float updateSpeed = 0.5f;
    [SerializeField] float positionOffset = 2f;

    MonsterHealth monsterHealth;
    
    // 헬스 바 로직 등록
    public void SetMonsterHealth(MonsterHealth monsterHealth)
    {
        this.monsterHealth = monsterHealth;
        monsterHealth.OnHealthPctChange += HandleHealthChange;
    }

    //헬스 바 로직
    void HandleHealthChange(float pct)
    {
        StartCoroutine(UpdateHealthBar(pct));
    }

    IEnumerator UpdateHealthBar(float pct)
    {
        float preChangePct = healthImage.fillAmount;
        float elapsed = 0;
        while (elapsed < updateSpeed)
        {
            elapsed += Time.deltaTime;
            healthImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed/updateSpeed);
            yield return null;
        }

        healthImage.fillAmount = pct;
    }
    
    
    //헬스 바 위치
    void LateUpdate()
    {
        if (monsterHealth == null) return;
        transform.position = Camera.main.WorldToScreenPoint(monsterHealth.transform.position + Vector3.up * positionOffset);
    }

    //헬스 바 로직 등록 취소
    private void OnDisable()
    {
        if (monsterHealth == null) return;
        monsterHealth.OnHealthPctChange -= HandleHealthChange;
    }
}
