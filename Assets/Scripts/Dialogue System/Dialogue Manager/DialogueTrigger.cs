using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public string conversationID;
    public bool canActivate;

    public bool autoStart = false;


    private void Start() {

        if (autoStart) {
            DialogueManager.instance.speakerObject = this.transform;
            DialogueManager.instance.StartConversation(conversationID);
        }

    }

    private void Update() {
        if (!autoStart) {
            if (DialogueManager.instance.m_input.NPCInteractUp || Input.GetKeyUp(KeyCode.Tab)) {
                if (DialogueManager.instance.cooldownTimer < 0 && canActivate && !DialogueManager.instance.inConversation) {
                    DialogueManager.instance.speakerObject = this.transform;
                    DialogueManager.instance.StartConversation(conversationID);
                }
            }
        }
    }

}