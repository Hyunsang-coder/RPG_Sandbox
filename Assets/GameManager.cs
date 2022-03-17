using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject GameOverCanvas;
    
    void OnAwake()
    {
        
    }
    void OnEnable()
    {
        Health.OnDeath += GameOverMenu;;
        GameOverCanvas.SetActive(false);
    }

    private void Start()
    {

        
    }

    void GameOverMenu()
    {
        GameOverCanvas.SetActive(true);
        //Instantiate(GameOverCanvas, GameOverCanvas.transform.position, Quaternion.identity);
    }
    
    public void ReloadScene()
    {

        GameOverCanvas.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
