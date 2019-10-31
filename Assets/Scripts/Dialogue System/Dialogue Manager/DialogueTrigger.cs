using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public string conversationID;
    public bool canActivate;

    public bool autoStart = false;

    DialogueManager dialogue;
    private void Start() {
        dialogue = DialogueManager.instance;

        if (autoStart) {
            dialogue.speakerObject = this.transform;
            dialogue.StartConversation(conversationID);
        }
    }

    private void Update() {
        if (!autoStart) {
            if (dialogue.m_input.NPCInteractUp || Input.GetKeyUp(KeyCode.Tab)) {
                if (dialogue.cooldownTimer < 0 && canActivate && !dialogue.inConversation) {
                    dialogue.speakerObject = this.transform;
                    dialogue.StartConversation(conversationID);
                }
            }
        }
    }

}