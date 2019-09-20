using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public string conversationID;
    public bool canActivate;

    private void Update() {

        if (DialogueManager.instance.m_input.NPCInteractUp || Input.GetKeyUp(KeyCode.Tab)) {
            if (DialogueManager.instance.cooldownTimer < 0 && canActivate && !DialogueManager.instance.inConversation) {
                DialogueManager.instance.speakerObject = this.transform;
                DialogueManager.instance.StartConversation(conversationID);
            }
        }
    }

}