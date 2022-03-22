using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject GameOverCanvas;
    public int flashBangQty;
    PlayerController playerController;
    PlayerUI playerUI;


    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerUI = FindObjectOfType<PlayerUI>();    
    }
    void OnEnable()
    {
        Health.OnDeath += GameOverMenu;;
        GameOverCanvas.SetActive(false);
        playerController.OnItemPickup += IncreaseFlashBang;
        playerController.OnThrow += DecreaseFlashBang;
    }


    void GameOverMenu()
    {
        GameOverCanvas.SetActive(true);
        
    }
    
    public void ReloadScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        GameOverCanvas.SetActive(false);
        SceneManager.LoadScene(currentScene);
    }

    void IncreaseFlashBang()
    {
        flashBangQty ++;
        playerUI.UpdateFlashBangQty();
    }

    void DecreaseFlashBang()
    {
        flashBangQty--;
        playerUI.UpdateFlashBangQty();
    }
}
