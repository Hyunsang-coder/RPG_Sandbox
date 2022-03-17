using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCBehavior : MonoBehaviour
{
    public Button triggerButton;
    public bool interactionReady;
    

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            interactionReady = true;
            triggerButton.image.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            interactionReady = false;
            triggerButton.image.enabled = false;
        }
    }
}
