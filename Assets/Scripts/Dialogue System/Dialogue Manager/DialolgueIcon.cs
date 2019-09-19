using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialolgueIcon : MonoBehaviour {

    public static DialolgueIcon instance;

    void Awake() {
        if (instance != null)
            return;
        instance = this;
    }

    public float lowerLimit = 2f;
    public float upperLimit = 10f;

    public float distanceToCharacter;
    public float percent;

    public DialogueTrigger previousClosestCharacter;
    public DialogueTrigger closestCharacter;

    public Canvas bubbleCanvas;

    public float iconHeight = 3f;

    public Vector3 initialScale;

    private void Start() {
        initialScale = bubbleCanvas.transform.localScale;
    }

    private void Update() {
        SenseCharactersNearby(1000f);

        if (closestCharacter != null) {
            percent = (distanceToCharacter - upperLimit) / (lowerLimit - upperLimit);
            if (percent < 0f) percent = 0f;
            else if (percent > 1f) percent = 1f;

            float newNum = initialScale.x * percent;
            bubbleCanvas.transform.localScale = new Vector3(newNum, newNum, newNum);

            if (previousClosestCharacter != closestCharacter) {
                previousClosestCharacter.canActivate = false;
                previousClosestCharacter = closestCharacter;
            }

            if (distanceToCharacter < 5f) closestCharacter.canActivate = true;
            else closestCharacter.canActivate = false;

            bubbleCanvas.transform.LookAt(Camera.main.transform, Vector3.up);
            bubbleCanvas.transform.localEulerAngles = new Vector3(0, bubbleCanvas.transform.localEulerAngles.y + 180, 0);
            bubbleCanvas.transform.position = new Vector3(closestCharacter.gameObject.transform.position.x, closestCharacter.gameObject.transform.position.y + iconHeight, closestCharacter.gameObject.transform.position.z);
        } else {
            bubbleCanvas.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void SenseCharactersNearby(float radius) {
        closestCharacter = GetClosestCharacter(Physics.OverlapSphere(transform.position, radius), radius);
        if (closestCharacter != null) distanceToCharacter = Vector3.Distance(transform.position, closestCharacter.gameObject.transform.position);
        if (!previousClosestCharacter) previousClosestCharacter = closestCharacter;
    }

    private DialogueTrigger GetClosestCharacter(Collider[] characters, float radius) {
        DialogueTrigger character = null;
        float minDist = radius;
        foreach (Collider currentCharacter in characters) {
            if (currentCharacter.gameObject == gameObject)
                continue;

            DialogueTrigger comp = currentCharacter.GetComponent<DialogueTrigger>();
            if (comp != null) {
                Vector3 t = currentCharacter.transform.position - transform.position;
                float dist = t.x * t.x + t.y * t.y + t.z * t.z;  // Same as "= t.sqrMagnitude;" but faster
                if (dist < minDist) {
                    character = comp;
                    minDist = dist;
                }
            }
        }
        return character;
    }

    public void SetVisible(bool isVisible) {
        if (isVisible)
            bubbleCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        else
            bubbleCanvas.GetComponent<CanvasGroup>().alpha = 0.0f;
    }
}
