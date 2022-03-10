using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] Image healthImage;
    [SerializeField] Image staminaImage;
    [SerializeField] float updateSpeed = 0.5f;

    Health playerHealth;

    // �ｺ �� ���� ���
    private void OnEnable()
    {
        playerHealth = FindObjectOfType<Health>();
        playerHealth.OnHealthPctChange += HandleHealthChange;
        playerHealth.OnStaminaPctChange += HandleStaminaChange;
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
            healthImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeed);
            yield return null;
        }

        healthImage.fillAmount = pct;
    }


    void HandleStaminaChange(float pct)
    {
        StartCoroutine(UpdateStaminaBar(pct));
    }

    IEnumerator UpdateStaminaBar(float pct)
    {
        float preChangePct = staminaImage.fillAmount;
        float elapsed = 0;
        while (elapsed < updateSpeed)
        {
            elapsed += Time.deltaTime;
            staminaImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeed);
            yield return null;
        }

        staminaImage.fillAmount = pct;
    }



    //�ｺ �� ���� ��� ���
    private void OnDisable()
    {
        playerHealth.OnHealthPctChange -= HandleHealthChange;
    }
}
