using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action<int> OnLevelUp = delegate { };

    [SerializeField] Canvas GameOverCanvas;
    public int flashBangQty = 0;
    public int potionQty = 0;
    public int playerLevel = 1;
    PlayerController playerController;
    PlayerUI playerUI;


    [SerializeField] int playerXP = 0;


    
    void OnEnable()
    {
        Health.OnDeath += GameOverMenu;;
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerUI = FindObjectOfType<PlayerUI>();
        GameOverCanvas = GameObject.FindWithTag("GameOverCanvas").GetComponent<Canvas>();
        GameOverCanvas.enabled = false;
    }

    public void GainExperience(int xp)
    {
        playerXP += xp;
        CheckLevel();
        playerUI.UpdateLv_ItemUI();
    }

    public void UsePostion()
    {
        potionQty--;
        playerUI.UpdateLv_ItemUI();
    }

    bool lv2First = true;
    bool lv3First = true;
    bool lv4First = true;
    bool lv5First = true;
    public void CheckLevel()
    {
        switch (playerXP)
        {
            case int xp when xp >= 700:
                playerLevel = 5;
                if (lv5First)
                {
                    OnLevelUp(playerLevel);
                    lv5First = false;
                }
                break;
            case int xp when xp >= 450:
                playerLevel = 4;
                if (lv4First)
                {
                    OnLevelUp(playerLevel);
                    lv4First = false;
                }
                break;
            case int xp when xp >= 250:
                playerLevel = 3;
                if (lv3First)
                {
                    OnLevelUp(playerLevel);
                    lv3First = false;
                }
                break;
            case int xp when xp >= 100:
                playerLevel = 2;
                if (lv2First)
                {
                    OnLevelUp(playerLevel);
                    lv2First = false;
                }
                break;
            default:
                playerLevel = 1;
                break;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReloadScene();
        }
    }


    void GameOverMenu()
    {
        GameOverCanvas.enabled = true;
    }
    
    public void ReloadScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        //GameOverCanvas.SetActive(false);
        SceneManager.LoadScene(currentScene);
    }

    private void OnDisable()
    {
        Health.OnDeath -= GameOverMenu;
    }

}
