using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject GameOverCanvas;
    
    void OnEnable()
    {
        Health.OnDeath += GameOverMenu;;
        GameOverCanvas.SetActive(false);
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
}
