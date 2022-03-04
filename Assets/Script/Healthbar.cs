using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Image healthImage;
    [SerializeField] float updateSpeed = 0.5f;


    private void Awake()
    {
        GetComponentInParent<Health>().OnHealthPercentChange += HandleHealthChange;
    }

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
    
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);

    }
}
