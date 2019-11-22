using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialolgueIcon : MonoBehaviour {

    public static DialolgueIcon instance;
    void Awake() {
        if (instance != null) return;
        instance = this;
    }

    public float iconHeight = 3f;
    private float checkTimer;

    public float lowerLimit = 2f;
    public float upperLimit = 10f;
    public float distanceToCharacter;

    public DialogueTrigger closestCharacter;
    public DialogueTrigger previousClosestCharacter;

    public Canvas bubbleCanvas;
    public Vector3 initialScale;

    private void Start() {
        initialScale = bubbleCanvas.transform.localScale;
    }

    private void Update() {

        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f) {
            checkTimer = 1f;
            SenseNearbyCharacters(20f);
        }

        if (closestCharacter != null) {
           // if (distanceToCharacter < 5f) closestCharacter.canActivate = true;
           // else closestCharacter.canActivate = false;

            bubbleCanvas.transform.localScale = CalculateDistance();
            bubbleCanvas.transform.LookAt(Camera.main.transform, Vector3.up);
            bubbleCanvas.transform.localEulerAngles = new Vector3(0, bubbleCanvas.transform.localEulerAngles.y + 180, 0);

        }
    }

    public void SenseNearbyCharacters(float radius) {
        closestCharacter = GetClosestCharacter(transform.position, radius);
        if (!previousClosestCharacter) previousClosestCharacter = closestCharacter;

        if (closestCharacter != null) {
            distanceToCharacter = Vector3.Distance(transform.position, closestCharacter.gameObject.transform.position);
            GameObject closestObject = closestCharacter.gameObject;
            bubbleCanvas.transform.position = new Vector3(closestObject.transform.position.x, closestObject.transform.position.y + iconHeight, closestObject.transform.position.z);

            if (previousClosestCharacter != closestCharacter) {
          //      previousClosestCharacter.canActivate = false;
                previousClosestCharacter = closestCharacter;
            }

        } else {
         //   closestCharacter.canActivate = false;
         //   previousClosestCharacter.canActivate = false;
            bubbleCanvas.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    private DialogueTrigger GetClosestCharacter(Vector3 pos, float radius) {
        Collider[] characters = Physics.OverlapSphere(transform.position, radius);
        DialogueTrigger character = null;
        float minDist = radius;
        foreach (Collider currentCharacter in characters) {
            if (currentCharacter.gameObject == gameObject) continue;

            DialogueTrigger comp = currentCharacter.GetComponent<DialogueTrigger>();
            if (comp != null) {
                if (!comp.autoStart) {
                    Vector3 t = currentCharacter.transform.position - transform.position;
                    float dist = t.x * t.x + t.y * t.y + t.z * t.z;
                    if (dist < minDist) {
                        character = comp;
                        minDist = dist;
                    }
                }
            }
        }

        return character;
    }
    Vector3 CalculateDistance() {
        float percent = (distanceToCharacter - upperLimit) / (lowerLimit - upperLimit);
        if (percent < 0f) percent = 0f;
        else if (percent > 1f) percent = 1f;
        return new Vector3(initialScale.x * percent, initialScale.x * percent, initialScale.x * percent);
    }

    public void SetVisible(bool isVisible) {
        if (isVisible) bubbleCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        else bubbleCanvas.GetComponent<CanvasGroup>().alpha = 0.0f;
    }

}
