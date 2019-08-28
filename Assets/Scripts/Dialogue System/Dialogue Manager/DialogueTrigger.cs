using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public string conversationID;
    public bool canActivate;

    private void Update() {
        if (DialogueManager.instance.m_input.NPCInteractDown || Input.GetKeyDown(KeyCode.Space)) {
            if (canActivate && !DialogueManager.instance.inConversation)
                DialogueManager.instance.StartConversation(conversationID);
        }
    }

    /*
    public void OnTriggerEnter(Collider other) {

        // Simple trigger that launces a conversation when the player touches the collider
        if (other.gameObject.GetComponent<DialogueCharacter>() && canActivate)
            DialogueManager.instance.StartConversation(conversationID);

    }
    */
}