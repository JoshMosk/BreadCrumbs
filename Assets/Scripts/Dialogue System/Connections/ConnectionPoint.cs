using System;
using UnityEditor;
using UnityEngine;

public class ConnectionPoint {

    public enum ConnectionPointType {
        In,
        Out
    }

    public Rect rect;
    public Node parentNode;
    public int totalNumber;
    public int optionNumber;
    public Node.NodeType nodeType;
    public ConnectionPointType connectionType;
    public Action<ConnectionPoint, ConnectionPointType> OnClickConnectionPoint;


    public ConnectionPoint(Node parentNode, ConnectionPointType connectionType, Action<ConnectionPoint, ConnectionPointType> OnClickConnectionPoint, Node.NodeType nodeType, int optionNumber, int totalNumber) {

        // Set all of the values for the new connection point to be created
        this.nodeType = nodeType;
        this.parentNode = parentNode;
        this.totalNumber = totalNumber;
        this.optionNumber = optionNumber;
        this.connectionType = connectionType;
        this.OnClickConnectionPoint = OnClickConnectionPoint;

    }

#if (UNITY_EDITOR)

    public void Draw() {

        rect = new Rect(0, 0, 10f, 25f);

        // Find Y Position of the connection points and if they need to be offset
        float defaultPosition = parentNode.nodeRect.y + (parentNode.nodeRect.height * 0.5f) - (rect.height * 0.5f);

        float height = rect.height;
        float totalSpaces = totalNumber + (totalNumber - 1);
        float totalHeight = (totalNumber * height) + ((totalNumber - 1) * height);
        rect.y = (defaultPosition - (totalHeight / 2)) + (totalHeight / totalSpaces * (optionNumber * 2)) - (height * 1.5f);


        // Find X Position of the connection points
        switch (connectionType) {
            case ConnectionPointType.In:
                rect.x = parentNode.nodeRect.x - rect.width + 8f;
                break;

            case ConnectionPointType.Out:
                rect.x = parentNode.nodeRect.x + parentNode.nodeRect.width - 8f;
                break;
        }


        // Draw the button on the window.
        switch (connectionType) {
            case ConnectionPointType.In:

                GUIStyle inStyle = new GUIStyle();
                inStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
                inStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
                inStyle.border = new RectOffset(4, 4, 12, 12);
                if (GUI.Button(rect, "", inStyle)) OnClickConnectionPoint(this, connectionType);
                break;

            case ConnectionPointType.Out:
                GUIStyle outStyle = new GUIStyle();
                outStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
                outStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
                outStyle.border = new RectOffset(4, 4, 12, 12);
                if (GUI.Button(rect, "", outStyle)) OnClickConnectionPoint(this, connectionType);
                break;

        }
    }

#endif

}