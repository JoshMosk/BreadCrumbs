using System;
using UnityEditor;
using UnityEngine;

public enum ConnectionPointType {In, Out}

public class ConnectionPoint {

    public Rect rect;
    public Node node;
    public bool boolType;
    public int optionNumber;
    public Node.NodeType typeNode;
    public ConnectionPointType type;
    public Action<ConnectionPoint, ConnectionPointType> OnClickConnectionPoint;

    public ConnectionPoint(Node node, ConnectionPointType type, Action<ConnectionPoint, ConnectionPointType> OnClickConnectionPoint, Node.NodeType typeNode, bool boolType, int optionNumber) {

        this.node = node;
        this.type = type;
        this.boolType = boolType;
        this.typeNode = typeNode;
        this.optionNumber = optionNumber;
        this.OnClickConnectionPoint = OnClickConnectionPoint;

    }

    public void Draw() {

         rect = new Rect(0, 0, 10f, 25f);

        // Find Y Position of the connection points and if they need to be offset
        float defaultPosition = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;
        if (typeNode == Node.NodeType.GetBooleanNode) {
            if (type == ConnectionPointType.In) rect.y = defaultPosition;
            else if (boolType) rect.y = defaultPosition - rect.height;
            else rect.y = defaultPosition + rect.height;

        } else if (typeNode == Node.NodeType.MultipleChoiceNode) {
            if (optionNumber == 1) rect.y = defaultPosition - (rect.height * 3);
            else if (optionNumber == 2) rect.y = defaultPosition - rect.height;
            else if (optionNumber == 3) rect.y = defaultPosition + rect.height;
            else if (optionNumber == 4) rect.y = defaultPosition + (rect.height * 3);
            else rect.y = defaultPosition;

        } else rect.y = defaultPosition;


        // Find X Position of the connection points
        switch (type) {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width + 8f;
                break;

            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width - 8f;
                break;
        }


        // Draw the button on the window.
        switch (type) {
            case ConnectionPointType.In:
                GUIStyle inStyle = new GUIStyle();
                inStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
                inStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
                inStyle.border = new RectOffset(4, 4, 12, 12);
                if (GUI.Button(rect, "", inStyle)) OnClickConnectionPoint(this, type);
                break;

            case ConnectionPointType.Out:
                GUIStyle outStyle = new GUIStyle();
                outStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
                outStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
                outStyle.border = new RectOffset(4, 4, 12, 12);
                if (GUI.Button(rect, "", outStyle)) OnClickConnectionPoint(this, type);
                break;
        }

    }
}