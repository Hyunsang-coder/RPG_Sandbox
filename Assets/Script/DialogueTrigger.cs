using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialougue;
    public Transform NPCTransform;
    public Vector3 offset = new Vector3();

    public void TriggerDialogue()
    {
        DialogueManager.instance.StartConversation(dialougue);
    }

    private void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(NPCTransform.position + offset);
    }
}
