using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    // ü�� �������� �� �̹���. ���� ���� ������ �󿡼� �Ҵ��� ����.
    [SerializeField] Image healthImage;
    [SerializeField] float updateSpeed = 2;

    // Action event�� �Լ� ��� 
    private void Awake()
    {
        GetComponentInParent<EnemyHealth>().OnHealthChange += HandleHealthChange;
    }

    // ��ϵǴ� �Լ� ����: �̹����� fillAmount�� healthPercent�� ���� �� 
    void HandleHealthChange(float healthPercent)
    {
        //StartCoroutine(UpdateHealthBar(healthPercent));
        healthImage.fillAmount = healthPercent;
    }

    IEnumerator UpdateHealthBar(float healthPercent)
    {
        float preChangePct = healthImage.fillAmount;
        float elapsed = 0;
        while (elapsed < updateSpeed)
        {
            elapsed += Time.deltaTime;
            healthImage.fillAmount = Mathf.Lerp(preChangePct, healthPercent, elapsed / updateSpeed);
            yield return null;
        }

        healthImage.fillAmount = healthPercent;
    }

    // ü�� �������� ȸ������ �ʰ� ȭ�� �� �����ϰ� ���̵��� ���� ����
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

}
