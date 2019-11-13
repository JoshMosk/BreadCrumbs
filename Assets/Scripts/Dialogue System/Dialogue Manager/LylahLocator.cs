using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LylahLocator : MonoBehaviour {

    public Canvas bubbleCanvas;
    public GameObject lylahObject;
    public float iconH;

    void Update() {

        if (Input.GetKey(KeyCode.A)) {
            bubbleCanvas.transform.LookAt(Camera.main.transform, Vector3.up);
            bubbleCanvas.transform.localEulerAngles = new Vector3(0, bubbleCanvas.transform.localEulerAngles.y + 180, 0);
            bubbleCanvas.transform.position = new Vector3(lylahObject.transform.position.x, lylahObject.transform.position.y + iconH, lylahObject.transform.position.z);
            bubbleCanvas.GetComponent<CanvasGroup>().alpha = 1;

        } else bubbleCanvas.GetComponent<CanvasGroup>().alpha = 0;
    }
}
