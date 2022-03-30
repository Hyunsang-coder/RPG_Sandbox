using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    // 체력 게이지가 될 이미지. 잊지 말고 에디터 상에서 할당해 주자.
    [SerializeField] Image healthImage;
    [SerializeField] float updateSpeed = 2;

    // Action event에 함수 등록 
    private void Awake()
    {
        GetComponentInParent<EnemyHealth>().OnHealthChange += HandleHealthChange;
    }

    // 등록되는 함수 내용: 이미지의 fillAmount를 healthPercent와 같게 함 
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

    // 체력 게이지가 회전하지 않고 화면 상에 일정하게 보이도록 방향 설정
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

}
