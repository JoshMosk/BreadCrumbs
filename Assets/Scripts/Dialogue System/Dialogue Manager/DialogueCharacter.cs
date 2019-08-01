using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueCharacter : MonoBehaviour {


    [Header("Dialogue System")]
    public string characterName;


    [Header("Debug Options")]
    public float debugTextHeight = 1f;
    public int debugFontSize = 15;


    void OnDrawGizmos() {

        Texture2D targetTexture = new Texture2D(1, 1);
        for (int y = 0; y < 1; y++) for (int x = 0; x < 1; x++) targetTexture.SetPixel(x, y, Color.black);
        targetTexture.Apply();

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = debugFontSize;
        style.normal.background = targetTexture;
        style.alignment = TextAnchor.MiddleCenter;
        Handles.Label(new Vector3(transform.position.x, transform.position.y + debugTextHeight, transform.position.z), characterName, style);

    }
}
