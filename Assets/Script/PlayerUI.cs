using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Image healthImage;
    [SerializeField] Image staminaImage;
    [SerializeField] float updateSpeed = 0.5f;

    [SerializeField] Text PotionTxt;
    [SerializeField] Text flashBangQtyTxt;
    
    
    
    [SerializeField] Text playerLvTxt;
    [SerializeField] Text HPTxt;
    [SerializeField] Text StaminaTxt;

    Health health;
    GameManager gameManager;
    PlayerController playerController;

    // 헬스 바 로직 등록
    private void OnEnable()
    {
        health = FindObjectOfType<Health>();
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();
        health.OnHealthPctChange += HandleHealthChange;
        health.OnStaminaPctChange += HandleStaminaChange;
    }

    private void Start()
    {
        UpdateLv_ItemUI();
        HandleHealthChange(100);
        HandleStaminaChange(100);
    }
    //헬스 바 로직
    void HandleHealthChange(float pct)
    {
        healthImage.fillAmount = pct;
        //StartCoroutine(UpdateHealthBar(pct));
        UpdateLv_ItemUI();
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
        staminaImage.fillAmount = pct;
        UpdateLv_ItemUI();
        //StartCoroutine(UpdateStaminaBar(pct));
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

    public void UpdateLv_ItemUI()
    {
        flashBangQtyTxt.text = "x " + gameManager.flashBangQty.ToString();
        PotionTxt.text = "x " + gameManager.potionQty.ToString();
        playerLvTxt.text = gameManager.playerLevel.ToString();
        HPTxt.text = health.currentHealth.ToString("0.#") + " / " + health.maxHealth.ToString();
        StaminaTxt.text = health.currentStamina.ToString("0.#") + " / " + health.maxStamina.ToString();
    }


    //헬스 바 로직 등록 취소
    private void OnDisable()
    {
        health.OnHealthPctChange -= HandleHealthChange;
        health.OnStaminaPctChange -= HandleStaminaChange;
    }
}
