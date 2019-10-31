﻿using System.Collections;
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
        SenseCharactersNearby(10f);

        if (closestCharacter != null) {
            if (distanceToCharacter < 5f) closestCharacter.canActivate = true;
            else closestCharacter.canActivate = false;

            bubbleCanvas.transform.localScale = CalculateDistance();
            bubbleCanvas.transform.LookAt(Camera.main.transform, Vector3.up);
            bubbleCanvas.transform.localEulerAngles = new Vector3(0, bubbleCanvas.transform.localEulerAngles.y + 180, 0);

        } else {
            bubbleCanvas.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void SenseCharactersNearby(float radius) {
        closestCharacter = GetClosestCharacter(Physics.OverlapSphere(transform.position, radius), radius);
        if (!previousClosestCharacter) previousClosestCharacter = closestCharacter;

        if (closestCharacter != null) {
            distanceToCharacter = Vector3.Distance(transform.position, closestCharacter.gameObject.transform.position);
            bubbleCanvas.transform.position = new Vector3(closestCharacter.gameObject.transform.position.x, closestCharacter.gameObject.transform.position.y + iconHeight, closestCharacter.gameObject.transform.position.z);

            if (previousClosestCharacter != closestCharacter) {
                previousClosestCharacter.canActivate = false;
                previousClosestCharacter = closestCharacter;
            }

        }
    }

    private DialogueTrigger GetClosestCharacter(Collider[] characters, float radius) {
        DialogueTrigger character = null;
        float minDist = radius;
        foreach (Collider currentCharacter in characters) {
            if (currentCharacter.gameObject == gameObject)
                continue;

            DialogueTrigger comp = currentCharacter.GetComponent<DialogueTrigger>();
            if (comp != null) {
                if (!comp.autoStart) {
                    Vector3 t = currentCharacter.transform.position - transform.position;
                    float dist = t.x * t.x + t.y * t.y + t.z * t.z;  // Same as "= t.sqrMagnitude;" but faster
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
        percent = (distanceToCharacter - upperLimit) / (lowerLimit - upperLimit);
        if (percent < 0f) percent = 0f;
        else if (percent > 1f) percent = 1f;

        float newNum = initialScale.x * percent;
        return new Vector3(newNum, newNum, newNum);
    }

    public void SetVisible(bool isVisible) {
        if (isVisible) bubbleCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        else bubbleCanvas.GetComponent<CanvasGroup>().alpha = 0.0f;
    }
}
