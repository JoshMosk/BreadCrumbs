using System;
using UnityEditor;
using UnityEngine;

public class Connection {

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public Action<Connection> removeConnection;


    public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> removeConnection) {

        // Set all of the values for the new connection to be created
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        this.removeConnection = removeConnection;

    }

    public void Draw() {

        // Get the line colour
        Color lineColor = new Color(91 / 255f, 169 / 255f, 193 / 255f);
        if (outPoint.parentNode.nodeType == Node.NodeType.GetBooleanNode) {
            if (outPoint.optionNumber == 1) lineColor = new Color(65 / 255f, 216 / 255f, 62 / 255f);
            else lineColor = new Color(216 / 255f, 62 / 255f, 73 / 255f);

        } else if (outPoint.parentNode.nodeType == Node.NodeType.MultipleChoiceNode) {
            //if (outPoint.optionNumber == 1) lineColor = new Color(216 / 255f, 62 / 255f, 160 / 255f);
            //else if (outPoint.optionNumber == 2) lineColor = new Color(167 / 255f, 216 / 255f, 62 / 255f);
            //else if (outPoint.optionNumber == 3) lineColor = new Color(72 / 255f, 62 / 255f, 216 / 255f);
            //else if (outPoint.optionNumber == 4) lineColor = new Color(175 / 255f, 62 / 255f, 216 / 255f);

        }

        // Shrink the curve if it is getting too close to the other node
        float curveLength = 100f;
        float difference = inPoint.rect.center.x - outPoint.rect.center.x;
        if (difference < curveLength) curveLength = difference;
        if (curveLength <= 0f) curveLength = 0f;

        // Draw the line in the editor
        Handles.DrawBezier(inPoint.rect.center, outPoint.rect.center, inPoint.rect.center + Vector2.left * curveLength, outPoint.rect.center - Vector2.left * curveLength, lineColor, null, 5f);


        // Draw the delete connection button on the middle of the line
        //if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 80, 12, Handles.CubeHandleCap)) removeConnection(this);
        GUIStyle buttonStyle = new GUIStyle();
        buttonStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn.png") as Texture2D;
        buttonStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn on.png") as Texture2D;

        float buttonSize = 15f;
        Vector2 pos = (inPoint.rect.center + outPoint.rect.center) * 0.5f;
        if (GUI.Button(new Rect(pos.x - (buttonSize/2), pos.y - (buttonSize / 2), buttonSize, buttonSize), "", buttonStyle)) {
            removeConnection(this);
        }
    }

}