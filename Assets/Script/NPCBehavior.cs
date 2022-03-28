using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCBehavior : MonoBehaviour
{
    public Button triggerButton;
    

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggerButton.image.enabled = true;

            var playerController = other.GetComponent<PlayerController>();
            playerController.isInteracting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggerButton.image.enabled = false;

            var playerController = other.GetComponent<PlayerController>();
            playerController.isInteracting = false;
            
            DialogueManager.instance.EndDialouge();
        }
    }
}
