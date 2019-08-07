using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Node {

    public enum NodeType {
        ConversationNode,
        DialogueNode,
        MultipleChoiceNode,
        EndNode,
        RandomNode,
        GetBooleanNode,
        SetBooleanNode,
        CommentNode
    }

    public string nodeID = "";

    public Rect rect;
    public string title;
    public bool isDragged;
    public bool isSelected;
    public NodeType type;

    public List<ConnectionPoint> inPoints = new List<ConnectionPoint>();
    public List<ConnectionPoint> outPoints = new List<ConnectionPoint>();

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


    public Node(Vector2 position, Action<ConnectionPoint, ConnectionPointType> OnClickPoint, Action<Node> OnClickRemoveNode, NodeType nodeType) {

        // Generate a unique ID for this node
        if (nodeID.Length == 0) for (int i = 0; i < 30; i++) nodeID += "abcdefghijklmnopqrstuvwxyz0123456789"[Random.Range(0, "abcdefghijklmnopqrstuvwxyz0123456789".Length)];

        // Set up the height and connection points
        type = nodeType;
        rect = new Rect(position.x, position.y, 200, (float)GetNodeInfo(type)["height"]);
        for (var i = 0; i < (int)GetNodeInfo(type)["inConnectionNumber"]; i++) inPoints.Add(new ConnectionPoint(this, ConnectionPointType.In, OnClickPoint, type, false, (int)GetNodeInfo(type)["inConnectionNumber"] + 1));
        for (var i = 0; i < (int)GetNodeInfo(type)["outConnectionNumber"]; i++) outPoints.Add(new ConnectionPoint(this, ConnectionPointType.Out, OnClickPoint, type, false, (int)GetNodeInfo(type)["outConnectionNumber"] + 1));

        // Create the style of the node for selected and unselected
        defaultNodeStyle = new GUIStyle();
        defaultNodeStyle.border = new RectOffset(12, 12, 12, 12);
        defaultNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0.png") as Texture2D;

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0 on.png") as Texture2D;

        // Set the current style to unselected
        style = defaultNodeStyle;
        OnRemoveNode = OnClickRemoveNode;

    }

    public Dictionary<string, object> GetNodeInfo(NodeType currentType) {

        // Setup each kind of node and its details
        Dictionary<string, object> nodeInfo = new Dictionary<string, object>();
        switch (currentType) {
            case NodeType.ConversationNode:
                nodeInfo.Add("name", "Conversation");
                nodeInfo.Add("description", "Begins a new thread of dialogue nodes");
                nodeInfo.Add("height", 105f);
                nodeInfo.Add("inConnectionNumber", 0);
                nodeInfo.Add("outConnectionNumber", 1);
                break;

            case NodeType.DialogueNode:
                nodeInfo.Add("name", "Dialogue");
                nodeInfo.Add("description", "Defines a speach bubble in a conversation");
                nodeInfo.Add("height", 200f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 1);
                break;

            case NodeType.CommentNode:
                nodeInfo.Add("name", "Comment");
                nodeInfo.Add("description", "Can be used to describe a conversation");
                nodeInfo.Add("height", 150f);
                nodeInfo.Add("inConnectionNumber", 0);
                nodeInfo.Add("outConnectionNumber", 0);
                break;

            case NodeType.EndNode:
                nodeInfo.Add("name", "End Conversation");
                nodeInfo.Add("description", "Concludes the preceding conversation");
                nodeInfo.Add("height", 57f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 0);
                break;

            case NodeType.GetBooleanNode:
                nodeInfo.Add("name", "Get Variable");
                nodeInfo.Add("description", "Returns true or false based on a blackboard value");
                nodeInfo.Add("height", 135f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 1);
                break;

            case NodeType.SetBooleanNode:
                nodeInfo.Add("name", "Set Variable");
                nodeInfo.Add("description", "Changes a variable on the specified blackboard");
                nodeInfo.Add("height", 170f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 2);
                break;

            case NodeType.MultipleChoiceNode:
                nodeInfo.Add("name", "Multiple Choice");
                nodeInfo.Add("description", "Allows different options for the player to choose from");
                nodeInfo.Add("height", 400f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 4);
                break;

            case NodeType.RandomNode:
                nodeInfo.Add("name", "Random");
                nodeInfo.Add("description", "Picks a random node");
                nodeInfo.Add("height", 57f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 1);
                break;
        }

        return nodeInfo;

    }

    public void Drag(Vector2 delta) {

        // Track the difference of the current movement
        rect.position += delta;

    }

    public void Draw() {

        // Draw all of the connection points onto the node
        foreach (ConnectionPoint point in inPoints) point.Draw();
        foreach (ConnectionPoint point in outPoints) point.Draw();

        // Create the node and set up the scroll view
        float insets = 20f;
        GUIContent content = new GUIContent("", (string)GetNodeInfo(type)["description"]);
        GUI.Box(rect, content, style);
        GUILayout.BeginArea(new Rect(rect.x + insets, rect.y + insets, rect.width - (insets * 2), rect.height - (insets * 2)));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(rect.width - (insets * 2)), GUILayout.Height(rect.height - (insets * 2)));

        // Make the text styles
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.normal.textColor = Color.white;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 10;

        GUIStyle bodyStyle = new GUIStyle();
        bodyStyle.normal.textColor = Color.white;
        bodyStyle.fontStyle = FontStyle.Normal;
        bodyStyle.fontSize = 10;

        // Create a title label
        GUILayout.Label(" " + (string)GetNodeInfo(type)["name"], titleStyle);
        GUILayout.Space(3);

        // Add the fields to each node
        switch (type) {
            case NodeType.ConversationNode:

                if (uniqueIDString.Length == 0) {
                    uniqueIDString = "";
                    string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
                    for (int i = 0; i < 10; i++) uniqueIDString += glyphs[Random.Range(0, glyphs.Length)];
                }

                GUILayout.Space(5);
                GUILayout.Label(" Unique ID: " + uniqueIDString, bodyStyle);
                GUILayout.Space(5);
                if (GUILayout.Button("Copy")) GUIUtility.systemCopyBuffer = uniqueIDString;

                break;


            case NodeType.DialogueNode:

                GUILayout.Space(5);
                GUILayout.Label(" Speaker", bodyStyle);
                titleString = GUILayout.TextField(titleString, 25);

                GUILayout.Space(5);
                GUILayout.Label(" Speech", bodyStyle);
                bodyString = GUILayout.TextArea(bodyString, GUILayout.ExpandHeight(true));
                GUILayout.Space(5);

                break;


            case NodeType.CommentNode:

                bodyString = GUILayout.TextArea(bodyString, GUILayout.ExpandHeight(true));
                GUILayout.Space(5);

                break;


            case NodeType.GetBooleanNode:

                GUILayout.Space(5);
                GUILayout.Label(" Blackboard", bodyStyle);
                blackboardString = GUILayout.TextField(blackboardString, 25);

                GUILayout.Space(5);
                GUILayout.Label(" Variable", bodyStyle);
                variableString = GUILayout.TextField(variableString, 25);

                break;


            case NodeType.SetBooleanNode:
               
                GUILayout.Space(5);
                GUILayout.Label(" Blackboard", bodyStyle);
                blackboardString = GUILayout.TextField(blackboardString, 25);

                GUILayout.Space(5);
                GUILayout.Label(" Variable", bodyStyle);
                variableString = GUILayout.TextField(variableString, 25);

                GUILayout.Space(5);
                GUILayout.Label(" Value", bodyStyle);
                valueBool = GUILayout.Toggle(valueBool, new GUIContent());

                break;


            case NodeType.MultipleChoiceNode:

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

                break;


        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();

    }

    public bool ProcessEvents(Event currentEvent) {

        // Process what happens when the mouse is clicked
        switch (currentEvent.type) {
            case EventType.MouseDown:
                if (currentEvent.button == 0) {
                    if (rect.Contains(currentEvent.mousePosition)) {
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

                if (currentEvent.button == 1 && isSelected && rect.Contains(currentEvent.mousePosition)) {
                    ProcessContextMenu();
                    currentEvent.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (currentEvent.button == 0 && isDragged) {
                    Drag(currentEvent.delta);
                    currentEvent.Use();
                    return true;
                }
                break;
        }

        return false;
    }

    private void ProcessContextMenu() {

        // Create the right click menu to remove a node
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();

    }

    private void OnClickRemoveNode() {

        OnRemoveNode(this);

    }

}