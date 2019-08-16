using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public string conversationID;


    public void OnTriggerEnter(Collider other) {

        // Simple trigger that launces a conversation when the player touches the collider
        if (other.gameObject.GetComponent<DialogueCharacter>()) DialogueManager.instance.StartConversation(conversationID);

    }

}