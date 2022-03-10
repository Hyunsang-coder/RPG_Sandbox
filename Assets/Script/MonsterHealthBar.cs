using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    [SerializeField] Image healthImage;
    [SerializeField] float updateSpeed = 0.5f;
    [SerializeField] float positionOffset = 2f;

    MonsterHealth monsterHealth;
    
    // �ｺ �� ���� ���
    public void SetMonsterHealth(MonsterHealth monsterHealth)
    {
        this.monsterHealth = monsterHealth;
        monsterHealth.OnHealthPctChange += HandleHealthChange;
    }

    //�ｺ �� ����
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
    
    
    //�ｺ �� ��ġ
    void LateUpdate()
    {
        if (monsterHealth == null) return;
        if (monsterHealth.gameObject.tag == "Dragon")
        {
            transform.position = Camera.main.WorldToScreenPoint(monsterHealth.transform.position + Vector3.up * positionOffset * 1.9f);
            transform.localScale = new Vector3 (2, 1.8f, 0);
        }
        else { transform.position = Camera.main.WorldToScreenPoint(monsterHealth.transform.position + Vector3.up * positionOffset); }
    }

    //�ｺ �� ���� ��� ���
    private void OnDisable()
    {
        if (monsterHealth == null) return;
        monsterHealth.OnHealthPctChange -= HandleHealthChange;
    }
}
