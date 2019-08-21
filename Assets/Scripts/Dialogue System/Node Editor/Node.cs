using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Collections;

[Serializable]
public class NodeData : SerializableDictionary<string, string> { }

[Serializable]
public class Node {

    public enum NodeType {
        EndNode,
        RandomNode,
        CommentNode,
        DialogueNode,
        SetBooleanNode,
        GetBooleanNode,
        ConversationNode,
        MultipleChoiceNode,
        BroadcastNode,
        SetNumberNode,
        GetNumberNode,
    }

    public Rect nodeRect;
    public Rect actualNodeRect;
    private bool isDragged;
    private bool isSelected;
    public NodeType nodeType;
    public string nodeID = "";
    private Vector2 scrollPosition;
    public string uniqueIDString = "";

    private GUIStyle style;
    private GUIStyle defaultNodeStyle;
    private GUIStyle selectedNodeStyle;

    public Action<Node> OnRemoveNode;

    public List<ConnectionPoint> inPoints = new List<ConnectionPoint>();
    public List<ConnectionPoint> outPoints = new List<ConnectionPoint>();
    public NodeData nodeData = new NodeData();
    public List<Dictionary<string, object>> nodeFields = new List<Dictionary<string, object>>();


    public Node(Vector2 position, Action<ConnectionPoint, ConnectionPoint.ConnectionPointType> OnClickPoint, Action<Node> OnClickRemoveNode, NodeType nodeType, bool isCentered) {

        // Generate a unique ID for this node
        if (nodeID.Length == 0) for (int i = 0; i < 30; i++) nodeID += "abcdefghijklmnopqrstuvwxyz0123456789"[Random.Range(0, "abcdefghijklmnopqrstuvwxyz0123456789".Length)];

        // Set up the height and connection points
        this.nodeType = nodeType;

        if (isCentered) nodeRect = new Rect(position.x - ((float)GetNodeInfo(nodeType)["width"]/2), position.y - ((float)GetNodeInfo(nodeType)["height"] / 2), (float)GetNodeInfo(nodeType)["width"], (float)GetNodeInfo(nodeType)["height"]);
        else nodeRect = new Rect(position.x, position.y, (float)GetNodeInfo(nodeType)["width"], (float)GetNodeInfo(nodeType)["height"]);

        actualNodeRect = nodeRect;
        for (var i = 0; i < (int)GetNodeInfo(nodeType)["inConnectionNumber"]; i++) inPoints.Add(new ConnectionPoint(this, ConnectionPoint.ConnectionPointType.In, OnClickPoint, nodeType, i + 1, (int)GetNodeInfo(nodeType)["inConnectionNumber"]));
        for (var i = 0; i < (int)GetNodeInfo(nodeType)["outConnectionNumber"]; i++) outPoints.Add(new ConnectionPoint(this, ConnectionPoint.ConnectionPointType.Out, OnClickPoint, nodeType, i + 1, (int)GetNodeInfo(nodeType)["outConnectionNumber"]));


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

        // Create a unique ID for this node if it has not already been made
        if (uniqueIDString.Length == 0) {
            uniqueIDString = "";
            string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < 10; i++) uniqueIDString += glyphs[Random.Range(0, glyphs.Length)];
            nodeData["uniqueID"] = uniqueIDString;
        }

        // Setup each kind of node and its details
        nodeFields = new List<Dictionary<string, object>>();
        Dictionary<string, object> nodeInfo = new Dictionary<string, object>();
        switch (currentType) {
            case NodeType.ConversationNode:

                nodeInfo.Add("name", "Conversation");
                nodeInfo.Add("description", "Begins a new thread of dialogue nodes");
                nodeInfo.Add("height", 138f);
                nodeInfo.Add("width", 180f);
                nodeInfo.Add("inConnectionNumber", 0);
                nodeInfo.Add("outConnectionNumber", 1);

                nodeFields.Add(new Dictionary<string, object> { { "name", "Name" }, { "kind", "SmallBox" }, { "id", "name" } });
                nodeFields.Add(new Dictionary<string, object> { {"name", "ID: " + uniqueIDString }, { "kind", "Copy" }, { "id", "uniqueID" } });

                break;


            case NodeType.DialogueNode:

                nodeInfo.Add("name", "Dialogue");
                nodeInfo.Add("description", "Defines a speach bubble in a conversation");
                nodeInfo.Add("height", 200f);
                nodeInfo.Add("width", 200f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 1);

                nodeFields.Add(new Dictionary<string, object> { { "name", "Speaker" }, { "kind", "Dropdown" }, { "id", "speaker" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Dialogue" }, { "kind", "LargeBox" }, { "id", "dialogue" } });

                break;


            case NodeType.CommentNode:

                nodeInfo.Add("name", "Comment");
                nodeInfo.Add("description", "Can be used to describe a conversation");
                nodeInfo.Add("height", 150f);
                nodeInfo.Add("width", 200f);
                nodeInfo.Add("inConnectionNumber", 0);
                nodeInfo.Add("outConnectionNumber", 0);

                nodeFields.Add(new Dictionary<string, object> { { "name", "" }, { "kind", "LargeBox" }, { "id", "comment" } });

                break;


            case NodeType.EndNode:

                nodeInfo.Add("name", "End Conversation");
                nodeInfo.Add("description", "Concludes the preceding conversation");
                nodeInfo.Add("height", 57f);
                nodeInfo.Add("width", 160f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 0);

                break;


            case NodeType.GetBooleanNode:

                nodeInfo.Add("name", "Get Boolean");
                nodeInfo.Add("description", "Returns true or false based on a blackboard value");
                nodeInfo.Add("height", 135f);
                nodeInfo.Add("width", 200f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 2);

                nodeFields.Add(new Dictionary<string, object> { { "name", "Blackboard" }, { "kind", "Dropdown" }, { "id", "blackboard" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Variable" }, { "kind", "Dropdown" }, { "id", "variable" } });

                break;


            case NodeType.SetBooleanNode:

                nodeInfo.Add("name", "Set Boolean");
                nodeInfo.Add("description", "Changes a boolean variable on the specified blackboard");
                nodeInfo.Add("height", 170f);
                nodeInfo.Add("width", 200f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 1);

                nodeFields.Add(new Dictionary<string, object> { { "name", "Blackboard" }, { "kind", "Dropdown" }, { "id", "blackboard" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Variable" }, { "kind", "Dropdown" }, { "id", "variable" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Value" }, { "kind", "Box" }, { "id", "value" } });

                break;


            case NodeType.GetNumberNode:

                nodeInfo.Add("name", "Get Number");
                nodeInfo.Add("description", "Returns true or false based on a comparison of a blackboard value");
                nodeInfo.Add("height", 135f);
                nodeInfo.Add("width", 200f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 2);

                nodeFields.Add(new Dictionary<string, object> { { "name", "Blackboard" }, { "kind", "Dropdown" }, { "id", "blackboard" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Variable" }, { "kind", "Dropdown" }, { "id", "variable" } });

                break;


            case NodeType.SetNumberNode:

                nodeInfo.Add("name", "Set Number");
                nodeInfo.Add("description", "Changes a number variable on the specified blackboard");
                nodeInfo.Add("height", 170f);
                nodeInfo.Add("width", 200f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 1);

                nodeFields.Add(new Dictionary<string, object> { { "name", "Blackboard" }, { "kind", "Dropdown" }, { "id", "blackboard" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Variable" }, { "kind", "Dropdown" }, { "id", "variable" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Value" }, { "kind", "Box" }, { "id", "value" } });

                break;


            case NodeType.MultipleChoiceNode:

                nodeInfo.Add("name", "Multiple Choice");
                nodeInfo.Add("description", "Allows different options for the player to choose from");
                nodeInfo.Add("height", 350f);
                nodeInfo.Add("width", 200f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 4);

                nodeFields.Add(new Dictionary<string, object> { { "name", "Speaker" }, { "kind", "Dropdown" }, { "id", "speaker" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Dialogue" }, { "kind", "LargeBox" }, { "id", "dialogue" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Option 1" }, { "kind", "SmallBox" }, { "id", "option1" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Option 2" }, { "kind", "SmallBox" }, { "id", "option2" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Option 3" }, { "kind", "SmallBox" }, { "id", "option3" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Option 4" }, { "kind", "SmallBox" }, { "id", "option4" } });

                break;


            case NodeType.RandomNode:

                nodeInfo.Add("name", "Random");
                nodeInfo.Add("description", "Picks a random node");
                nodeInfo.Add("height", 57f);
                nodeInfo.Add("width", 150f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 1);

                break;


            case NodeType.BroadcastNode:

                nodeInfo.Add("name", "Broadcast");
                nodeInfo.Add("description", "Calls custom scripting");
                nodeInfo.Add("height", 100f);
                nodeInfo.Add("width", 200f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 1);

                nodeFields.Add(new Dictionary<string, object> { { "name", "Event" }, { "kind", "SmallBox" }, { "id", "event" } });

                break;


        }

        return nodeInfo;

    }

    public void Draw() {

        // Draw all of the connection points onto the node
        foreach (ConnectionPoint inPoint in inPoints) {
            inPoint.Draw();
        }

        foreach (ConnectionPoint outPoint in outPoints) {
            outPoint.Draw();
        }


        // Create the node and set up the scroll view
        float insets = 20f;
        //GUI.Box(nodeRect, new GUIContent("", (string)GetNodeInfo(nodeType)["description"]), style);
        GUI.Box(nodeRect, new GUIContent(""), style);
        GUILayout.BeginArea(new Rect(nodeRect.x + insets, nodeRect.y + insets, nodeRect.width - (insets * 2), nodeRect.height - (insets * 2)));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(nodeRect.width - (insets * 2)), GUILayout.Height(nodeRect.height - (insets * 2)));


        // Make the text styles
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.normal.textColor = Color.white;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleLeft;
        if ((string)GetNodeInfo(nodeType)["name"] == "End Conversation" || (string)GetNodeInfo(nodeType)["name"] == "Random") titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontSize = 10;

        GUIStyle bodyStyle = new GUIStyle();
        bodyStyle.normal.textColor = Color.white;
        bodyStyle.fontStyle = FontStyle.Normal;
        bodyStyle.fontSize = 9;

        GUIStyle smallStyle = new GUIStyle();
        smallStyle.normal.textColor = Color.gray;
        smallStyle.fontStyle = FontStyle.Normal;
        smallStyle.alignment = TextAnchor.MiddleLeft;
        if ((string)GetNodeInfo(nodeType)["name"] == "End Conversation" || (string)GetNodeInfo(nodeType)["name"] == "Random") smallStyle.alignment = TextAnchor.MiddleCenter;
        smallStyle.fontSize = 9;

        GUI.skin.textField.fontSize = 11;


        // Add all of the fields to the node
        if ((string)GetNodeInfo(nodeType)["name"] == "End Conversation" || (string)GetNodeInfo(nodeType)["name"] == "Random") GUILayout.Label("" + (string)GetNodeInfo(nodeType)["name"], titleStyle);
        else GUILayout.Label(" " + (string)GetNodeInfo(nodeType)["name"], titleStyle);
        GUILayout.Space(3);

        foreach (Dictionary<string, object> fieldDict in nodeFields) {

            GUILayout.Space(5);

            if ((string)fieldDict["kind"] == "Label") {
                GUILayout.Label(" " + fieldDict["name"], bodyStyle);

            } else if ((string)fieldDict["kind"] == "LargeBox") {
                string nameString = (string)fieldDict["name"];
                if (nameString.Length != 0) GUILayout.Label(" " + fieldDict["name"], bodyStyle);
                string idString = (string)fieldDict["id"];
                if (!nodeData.ContainsKey(idString)) nodeData[idString] = "";
                nodeData[idString] = GUILayout.TextArea((string)nodeData[idString], GUILayout.ExpandHeight(true));

            } else if ((string)fieldDict["kind"] == "SmallBox") {
                GUILayout.Label(" " + fieldDict["name"], bodyStyle);
                string idString = (string)fieldDict["id"];
                if (!nodeData.ContainsKey(idString)) nodeData[idString] = "";
                nodeData[idString] = GUILayout.TextField((string)nodeData[idString], GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));

            } else if ((string)fieldDict["kind"] == "Copy") {
                GUILayout.Label(" " + fieldDict["name"], bodyStyle);
                string idString = (string)fieldDict["id"];
                if (!nodeData.ContainsKey(idString)) nodeData[idString] = "";
                if (GUILayout.Button("Copy")) GUIUtility.systemCopyBuffer = uniqueIDString;

            } else if ((string)fieldDict["kind"] == "Box") {
                string idString = (string)fieldDict["id"];
                if (!nodeData.ContainsKey(idString)) nodeData[idString] = "True";
                GUILayout.Label(" " + fieldDict["name"], bodyStyle);

                bool boolResult = BoolFromString(nodeData[idString]);
                boolResult = GUILayout.Toggle(boolResult, new GUIContent());
                nodeData[idString] = StringFromBool(boolResult); 

            } else if ((string)fieldDict["kind"] == "Dropdown") {
                string idString = (string)fieldDict["id"];
                List<string> dataList = new List<string>();
                foreach (Node nameNode in NodeBasedEditor.Instance.nodes) {
                    if (nameNode.nodeData.ContainsKey(idString)) {
                        if (!dataList.Contains((string)nameNode.nodeData[idString])) {
                            dataList.Add((string)nameNode.nodeData[idString]);
                        }
                    }
                }
                dataList.Sort();
                dataList.Insert(0, "-");

                GUILayout.Label(" " + fieldDict["name"], bodyStyle);
                EditorGUILayout.BeginHorizontal();

                if (!nodeData.ContainsKey(idString)) nodeData[idString] = "";
                nodeData[idString] = GUILayout.TextArea((string)nodeData[idString], GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));

                string selectedItem = dataList[EditorGUILayout.Popup(0, dataList.ToArray(), GUILayout.Width(20f))];
                if (selectedItem != "-") nodeData[idString] = selectedItem;
                EditorGUILayout.EndHorizontal();

            }

        }

        if (nodeFields.Count != 0) GUILayout.Space(5);

        GUILayout.EndScrollView();
        GUILayout.EndArea();

    }

    public bool ProcessEvents(Event currentEvent) {

        // Process what happens when the mouse is clicked
        switch (currentEvent.type) {
            case EventType.MouseDown:
                if (currentEvent.button == 0) {

                    if (currentEvent.clickCount == 1) {
                        if (nodeRect.Contains(currentEvent.mousePosition)) {
                            isDragged = true;
                            GUI.changed = true;
                            isSelected = true;
                            style = selectedNodeStyle;
                        } else {
                            GUI.changed = true;
                            isSelected = false;
                            style = defaultNodeStyle;
                            GUI.FocusControl(null);
                        }

                    } else if (currentEvent.clickCount == 2) {
                        if (nodeRect.Contains(currentEvent.mousePosition)) {
                            float nodeCenterX = nodeRect.position.x + (nodeRect.width / 2);
                            float nodeCenterY = nodeRect.position.y + (nodeRect.height / 2);
                            NodeBasedEditor.Instance.ScrollToNode(nodeCenterX, nodeCenterY);
                        }
                    }

                }

                if (currentEvent.button == 1 && isSelected && nodeRect.Contains(currentEvent.mousePosition)) {
                    ProcessContextMenu();
                    currentEvent.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                actualNodeRect = nodeRect;
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

        // When the user pressed 'Remove', get rid of the node
        OnRemoveNode(this);

    }

    public void DragAll(Vector2 delta) {

        actualNodeRect.position += delta;
        nodeRect.position += delta;

    }

    public void Drag(Vector2 delta) {

        actualNodeRect.position += delta;

        float snapRange = 15f;
        float desiredX = actualNodeRect.position.x;
        float desiredY = actualNodeRect.position.y;


        List<Node> snappableXNodes = new List<Node>();
        foreach (Node snapNode in NodeBasedEditor.Instance.nodes) {
            if (snapNode != this) {
                float xSnap = (actualNodeRect.position.x + (actualNodeRect.width / 2)) - (snapNode.nodeRect.position.x + (snapNode.nodeRect.width / 2));
                if (xSnap < snapRange && xSnap > -snapRange) snappableXNodes.Add(snapNode);
            }
        }

        if (snappableXNodes.Count != 0) {
            float closestDistance = 10000;
            Node closestNode = snappableXNodes[0];
            foreach (Node snapNode in snappableXNodes) {
                if (Vector2.Distance(new Vector2(actualNodeRect.x, actualNodeRect.y), new Vector2(snapNode.actualNodeRect.x, snapNode.actualNodeRect.y)) < closestDistance) {
                    closestNode = snapNode;
                    closestDistance = Vector2.Distance(new Vector2(actualNodeRect.x, actualNodeRect.y), new Vector2(snapNode.actualNodeRect.x, snapNode.actualNodeRect.y));
                }
            }

            desiredX = closestNode.nodeRect.position.x + ((closestNode.nodeRect.width - nodeRect.width) / 2);
        }


        List<Node> snappableYNodes = new List<Node>();
        foreach (Node snapNode in NodeBasedEditor.Instance.nodes) {
            if (snapNode != this) {
                float ySnap = (actualNodeRect.position.y + (actualNodeRect.height / 2)) - (snapNode.nodeRect.position.y + (snapNode.nodeRect.height / 2));
                if (ySnap < snapRange && ySnap > -snapRange) snappableYNodes.Add(snapNode);
            }
        }

        if (snappableYNodes.Count != 0) {
            float closestDistance = 10000;
            Node closestNode = snappableYNodes[0];
            foreach (Node snapNode in snappableYNodes) {
                if (Vector2.Distance(new Vector2(actualNodeRect.x, actualNodeRect.y), new Vector2(snapNode.actualNodeRect.x, snapNode.actualNodeRect.y)) < closestDistance) {
                    closestNode = snapNode;
                    closestDistance = Vector2.Distance(new Vector2(actualNodeRect.x, actualNodeRect.y), new Vector2(snapNode.actualNodeRect.x, snapNode.actualNodeRect.y));
                }
            }

            desiredY = closestNode.nodeRect.position.y + ((closestNode.nodeRect.height - nodeRect.height) / 2);
        }

        nodeRect.position = new Vector2(desiredX, desiredY);

    }

    public void MoveTo(Vector2 delta) {

        Vector2 newPos = nodeRect.position;
        newPos += delta;
        nodeRect.position = newPos;
        actualNodeRect.position = newPos;
        GUI.changed = true;

    }

    public static string StringFromBool(bool boolValue) {
        if (boolValue) return "True";
        else return "False";

    }

    public static bool BoolFromString(string stringValue) {
        if (stringValue == "True") return true;
        else return false;
    }

}