using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] Image healthImage;
    [SerializeField] float updateSpeed = 0.5f;

    Health playerHealth;

    // 헬스 바 로직 등록
    private void OnEnable()
    {
        playerHealth = FindObjectOfType<Health>();
        playerHealth.OnHealthPctChange += HandleHealthChange;
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
            healthImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeed);
            yield return null;
        }

        healthImage.fillAmount = pct;
    }


   
    //헬스 바 로직 등록 취소
    private void OnDisable()
    {
        playerHealth.OnHealthPctChange -= HandleHealthChange;
    }
}
