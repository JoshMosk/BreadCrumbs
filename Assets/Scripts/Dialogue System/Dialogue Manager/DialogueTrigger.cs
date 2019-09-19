using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public string conversationID;
    public bool canActivate;

    private void Update() {

        if (DialogueManager.instance.m_input.NPCInteractDown || Input.GetKeyDown(KeyCode.Space)) {
            if (DialogueManager.instance.cooldownTimer < 0 && canActivate && !DialogueManager.instance.inConversation) {
                DialogueManager.instance.speakerObject = this.transform;
                DialogueManager.instance.StartConversation(conversationID);
            }
        }
    }

}