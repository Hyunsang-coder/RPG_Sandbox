using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Image healthImage;
    [SerializeField] Image staminaImage;
    [SerializeField] float updateSpeed = 0.5f;
    [SerializeField] Text flashBangQty;

    Health playerHealth;
    GameManager gameManager;
    PlayerController playerController;

    // 헬스 바 로직 등록
    private void OnEnable()
    {
        playerHealth = FindObjectOfType<Health>();
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();
        playerHealth.OnHealthPctChange += HandleHealthChange;
        playerHealth.OnStaminaPctChange += HandleStaminaChange;
    }

    private void Start()
    {
        UpdateFlashBangQty();
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

    public void UpdateFlashBangQty()
    {
        flashBangQty.text = "x " + gameManager.flashBangQty.ToString();
    }


    //헬스 바 로직 등록 취소
    private void OnDisable()
    {
        playerHealth.OnHealthPctChange -= HandleHealthChange;
        playerHealth.OnStaminaPctChange -= HandleStaminaChange;
    }
}
