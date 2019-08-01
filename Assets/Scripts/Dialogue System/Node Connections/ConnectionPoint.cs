using System;
using UnityEditor;
using UnityEngine;

public enum ConnectionPointType {In, Out}

public class ConnectionPoint {

    public Rect rect;
    public Node node;
    public bool boolType;
    public GUIStyle style;
    public int optionNumber;
    public string typeString = "";
    public ConnectionPointType type;
    public Action<ConnectionPoint> OnClickConnectionPoint;

    public ConnectionPoint(Node node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint, string typeString, bool boolType, int optionNumber) {
        this.node = node;
        this.type = type;
        this.style = style;
        this.boolType = boolType;
        this.typeString = typeString;
        this.optionNumber = optionNumber;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        rect = new Rect(0, 0, 10f, 20f);
    }

    public void Draw() {

        // Y Position of the connection points.
        // If the node is of Boolean type, then create two connections and offset them.
        if (type == ConnectionPointType.Out) {
            if (typeString == "Boolean") {
                if (boolType) rect.y = (node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f) - 20;
                else rect.y = (node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f) + 20;

            } else if (typeString == "Multiple Choice") {
                if (optionNumber == 1) rect.y = (node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f) - 60;
                else if (optionNumber == 2) rect.y = (node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f) - 20;
                else if (optionNumber == 3) rect.y = (node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f) + 20;
                else rect.y = (node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f) + 60;

            } else rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;
        } else rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

        // X Position of the connection points.
        switch (type) {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width + 8f;
                break;

            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width - 8f;
                break;
        }

        // Draw the button on the window.
        if (GUI.Button(rect, "", style)) {
            if (OnClickConnectionPoint != null) OnClickConnectionPoint(this);
        }
    }
}