using UnityEngine;
using UnityEditor;

public class DialogueCharacter : MonoBehaviour {

    [Header("Dialogue")]
    public new string name;

    [Header("Debug")]
    public int fontSize = 8;
    public float labelHeight = 1f;

#if (UNITY_EDITOR)

    void OnDrawGizmos() {

        // Create a debug label to show who the character is in the scene view
        //Texture2D targetTexture = new Texture2D(1, 1);
        //for (int y = 0; y < 1; y++) for (int x = 0; x < 1; x++) targetTexture.SetPixel(x, y, Color.black);
        //targetTexture.Apply();

        //GUIStyle style = new GUIStyle();
        //style.normal.textColor = Color.white;
        //style.fontSize = fontSize;
        //style.normal.background = targetTexture;
        //style.alignment = TextAnchor.MiddleCenter;

        //Handles.Label(new Vector3(transform.position.x, transform.position.y + labelHeight, transform.position.z), name, style);

    }

#endif

}