using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public string conversationID;

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<DialogueCharacter>()) {
            DialogueManager.instance.StartConversation(conversationID);
        }
    }

}
