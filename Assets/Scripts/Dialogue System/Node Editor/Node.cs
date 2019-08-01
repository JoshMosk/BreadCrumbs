using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Node {
    public string nodeID = "";

    public Rect rect;
    public string title;
    public bool isDragged;
    public bool isSelected;
    public string type;

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public ConnectionPoint outPoint2;
    public ConnectionPoint outPoint3;
    public ConnectionPoint outPoint4;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    // Dialogue
    public string titleString = "";
    public string bodyString = "";

    // Conversation
    public string uniqueIDString = "";

    // Bool
    public string blackboardString = "";
    public string variableString = "";
    public bool valueBool = false;

    // Multiple Choice
    public string option1String = "";
    public string option2String = "";
    public string option3String = "";
    public string option4String = "";


    Vector2 scrollPosition;

    public Action<Node> OnRemoveNode;

    public Node(Vector2 position, float width, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, string typeString) {
        if (nodeID.Length == 0) {
            string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < 30; i++) nodeID += glyphs[Random.Range(0, glyphs.Length)];
        }

        type = typeString;

        float height = 0f;
        if (typeString == "Conversation") height = 120f;
        else if (typeString == "Dialogue") height = 200f;
        else if (typeString == "Random") height = 57f;
        else if (typeString == "Boolean") height = 135f;
        else if (typeString == "Set Variable") height = 170f;
        else if (typeString == "End Conversation") height = 57f;
        else if (typeString == "Comment") height = 150f;
        else if (typeString == "Multiple Choice") height = 400f;

        rect = new Rect(position.x, position.y, width, height);

        style = nodeStyle;

        if (typeString != "Comment") {
            if (typeString != "Conversation") inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint, typeString, false, 0);

            if (typeString == "Boolean") {
                outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, typeString, true, 0);
                outPoint2 = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, typeString, false, 0);
            }
            else if (typeString == "Multiple Choice")
            {
                outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, typeString, false, 1);
                outPoint2 = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, typeString, false, 2);
                outPoint3 = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, typeString, false, 3);
                outPoint4 = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, typeString, false, 4);
            }
            else if (typeString != "End Conversation")
            {
                outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, typeString, false, 0);
            }
        }


        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;
    }

    public void Drag(Vector2 delta) {
        rect.position += delta;
    }

    public void Draw() {
        if (inPoint != null) inPoint.Draw();
        if (outPoint != null) outPoint.Draw();
        if (type == "Boolean") outPoint2.Draw();
        if (type == "Multiple Choice")
        {
            outPoint2.Draw();
            outPoint3.Draw();
            outPoint4.Draw();
        }

        string tooltipString = "";
        if (type == "Conversation") tooltipString = "Begins a new thread of dialogue nodes";
        else if (type == "Dialogue") tooltipString = "Defines a speach bubble in a conversation";
        else if (type == "Random") tooltipString = "Picks a random node";
        else if (type == "Boolean") tooltipString = "Returns true or false based on a blackboard value";
        else if (type == "End Conversation") tooltipString = "Concludes the preceding conversation";
        else if (type == "Set Variable") tooltipString = "Changes a variable on the specified blackboard";
        else if (type == "Multiple Choice") tooltipString = "Allows different options for the player to choose from";

        GUIContent content = new GUIContent("", tooltipString);

        GUI.Box(rect, content, style);

        float insets = 20f;
          GUILayout.BeginArea(new Rect(rect.x + insets, rect.y + insets, rect.width - (insets * 2), rect.height - (insets * 2)));

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(rect.width - (insets * 2)), GUILayout.Height(rect.height - (insets * 2)));

        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontSize = 10;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.white;

        GUIStyle bodyStyle = new GUIStyle();
        bodyStyle.fontSize = 10;
        bodyStyle.fontStyle = FontStyle.Normal;
        bodyStyle.normal.textColor = Color.white;

        GUILayout.Label(" " + type, titleStyle);
        GUILayout.Space(5);

        if (type == "Conversation") {
            GUILayout.Space(5);
            GUILayout.Label(" Unique ID", bodyStyle);
            uniqueIDString = GUILayout.TextField(uniqueIDString, 10);
            if (GUILayout.Button("Generate")) {
                uniqueIDString = "";
                string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
                for (int i = 0; i < 10; i++) uniqueIDString += glyphs[Random.Range(0, glyphs.Length)];
            }

        }
        else if (type == "Dialogue")
        {
            GUILayout.Space(5);
            GUILayout.Label(" Speaker", bodyStyle);
            titleString = GUILayout.TextField(titleString, 25);

            GUILayout.Space(5);
            GUILayout.Label(" Speech", bodyStyle);
            bodyString = GUILayout.TextArea(bodyString, GUILayout.ExpandHeight(true));
            GUILayout.Space(5);

        } else if (type == "Multiple Choice") {
            GUILayout.Space(5);
            GUILayout.Label(" Speaker", bodyStyle);
            titleString = GUILayout.TextField(titleString, 25);

            GUILayout.Space(5);
            GUILayout.Label(" Speech", bodyStyle);
            bodyString = GUILayout.TextArea(bodyString, GUILayout.ExpandHeight(true));
            GUILayout.Space(5);

            GUILayout.Space(5);
            GUILayout.Label(" Option 1", bodyStyle);
            option1String = GUILayout.TextArea(option1String, GUILayout.ExpandHeight(false));
            GUILayout.Space(5);

            GUILayout.Space(5);
            GUILayout.Label(" Option 2", bodyStyle);
            option2String = GUILayout.TextArea(option2String, GUILayout.ExpandHeight(false));
            GUILayout.Space(5);

            GUILayout.Space(5);
            GUILayout.Label(" Option 3", bodyStyle);
            option3String = GUILayout.TextArea(option3String, GUILayout.ExpandHeight(false));
            GUILayout.Space(5);

            GUILayout.Space(5);
            GUILayout.Label(" Option 4", bodyStyle);
            option4String = GUILayout.TextArea(option4String, GUILayout.ExpandHeight(false));
            GUILayout.Space(5);

        } else if (type == "Random") {

        } else if (type == "End Conversation") {

        } else if (type == "Comment") {
            bodyString = GUILayout.TextArea(bodyString, GUILayout.ExpandHeight(true));
            GUILayout.Space(5);

        } else if (type == "Boolean") {
            GUILayout.Space(5);
            GUILayout.Label(" Blackboard", bodyStyle);
            blackboardString = GUILayout.TextField(blackboardString, 25);

            GUILayout.Space(5);
            GUILayout.Label(" Variable", bodyStyle);
            variableString = GUILayout.TextField(variableString, 25);

        } else if (type == "Set Variable") {
            GUILayout.Space(5);
            GUILayout.Label(" Blackboard", bodyStyle);
            blackboardString = GUILayout.TextField(blackboardString, 25);

            GUILayout.Space(5);
            GUILayout.Label(" Variable", bodyStyle);
            variableString = GUILayout.TextField(variableString, 25);

            GUILayout.Space(5);
            GUILayout.Label(" Value", bodyStyle);
            valueBool = GUILayout.Toggle(valueBool, new GUIContent());

        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();

       // GUILayout.Box("This is an sized label");
    }

    public bool ProcessEvents(Event e) {
        switch (e.type) {
            case EventType.MouseDown:
                if (e.button == 0) {
                    if (rect.Contains(e.mousePosition)) {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    } else {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }

                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition)) {
                    ProcessContextMenu();
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged) {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }

    private void ProcessContextMenu() {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode() {
        if (OnRemoveNode != null) {
            OnRemoveNode(this);
        }
    }
}