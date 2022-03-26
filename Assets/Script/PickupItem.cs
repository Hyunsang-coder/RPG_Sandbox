using System;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName;
    public int qty;
    public int effect;
    bool pickupReady;

    GameManager gameManager;
    PlayerUI playerUI;

    private void OnEnable()
    {
       gameManager = FindObjectOfType<GameManager>();
       playerUI = FindObjectOfType<PlayerUI>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pickupReady = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ObtainItem();
        }
    }

    private void ObtainItem()
    {
        if (pickupReady)
        {
            switch (itemName) 
            {
                case "FlashBang":
                    gameManager.flashBangQty++;
                    playerUI.UpdateLv_ItemUI();
                    break;
                case "HealthPotion":
                    gameManager.potionQty++;
                    playerUI.UpdateLv_ItemUI();
                    break;
            }
            
            Destroy(gameObject);
        }
        
    }
}