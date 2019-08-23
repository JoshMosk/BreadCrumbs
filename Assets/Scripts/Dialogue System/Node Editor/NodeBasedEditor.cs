using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class NodeBasedEditor : EditorWindow {

    public static NodeBasedEditor Instance { get; private set; }

    private List<Connection> connections;
    public List<Node> nodes = new List<Node>();

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 drag;
    private Vector2 offset;

    private string filePath = "";
    private string currentConversation;


    [MenuItem("Dialogue/Open Editor", false, 0)]
    public static void OpenEditor() {

        // Open the editor window in Unity
        NodeBasedEditor window = GetWindow<NodeBasedEditor>();
        window.titleContent = new GUIContent("Dialogue");

    }

    [MenuItem("Dialogue/New Chapter", false, 100)]
    public static void NewFile() {

        // Clear all of the data currently loaded
        GetWindow<NodeBasedEditor>().connections = new List<Connection>();
        GetWindow<NodeBasedEditor>().nodes = new List<Node>();
        GetWindow<NodeBasedEditor>().filePath = "";

    }

    [MenuItem("Dialogue/Load Chapter...", false, 100)]
    public static void LoadFile() {

        // Get the file path and load in the data
        GetWindow<NodeBasedEditor>().filePath = EditorUtility.OpenFilePanel("Load File", "Assets/StreamingAssets", "json");
        if (GetWindow<NodeBasedEditor>().filePath.Length != 0) GetWindow<NodeBasedEditor>().LoadData();

    }

    [MenuItem("Dialogue/Save", false, 200)]
    public static void SaveFile() {

        // Show the file explore to pick where to save the file if there isn't already a path
        if (GetWindow<NodeBasedEditor>().filePath.Length == 0) GetWindow<NodeBasedEditor>().filePath = EditorUtility.SaveFilePanel("Save As...", "Assets/StreamingAssets", "Untitled" + ".json", "json");
        GetWindow<NodeBasedEditor>().SaveData();

    }

    [MenuItem("Dialogue/Save As...", false, 200)]
    public static void SaveAsFile() {

        // Show the file explore to pick where to save the file
        GetWindow<NodeBasedEditor>().filePath = EditorUtility.SaveFilePanel("Save As...", "Assets/StreamingAssets", "Untitled" + ".json", "json");
        GetWindow<NodeBasedEditor>().SaveData();

    }


    private void OnEnable() {

        // Load in the data when the window is opened
        Instance = this;
        LoadData();

    }

    private void OnGUI() {

        // Create a texture with a desired background colour
        Texture2D backgroundColour = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        backgroundColour.SetPixel(0, 0, new Color(32 / 255f, 32 / 255f, 32 / 255f));
        backgroundColour.Apply();
        GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), backgroundColour, ScaleMode.StretchToFill);

        // Draw the grid onto the background
        DrawGrid(20, 0.15f, Color.gray);
        DrawGrid(60, 0.2f, Color.gray);

        // Create a texture with a desired background colour
        Texture2D panelColour = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        panelColour.SetPixel(0, 0, new Color(50 / 255f, 50 / 255f, 50 / 255f));
        panelColour.Apply();
        GUI.DrawTexture(new Rect(0, 0, maxSize.x, 32), panelColour, ScaleMode.StretchToFill);

        // Draw all of the connections and nodes
        if (connections != null) for (int i = 0; i < connections.Count; i++) connections[i].Draw();
        if (nodes != null) for (int i = 0; i < nodes.Count; i++) nodes[i].Draw();
        DrawConnectionLine(Event.current);

        // Process events for when the user interacts with the window
        if (nodes != null) for (int i = nodes.Count - 1; i >= 0; i--) GUI.changed = nodes[i].ProcessEvents(Event.current);
        ProcessEvents(Event.current);

        // Create the jump to conversation panel
        GUIStyle bodyStyle = new GUIStyle();
        bodyStyle.normal.textColor = Color.white;
        bodyStyle.fontStyle = FontStyle.Normal;
        bodyStyle.fontSize = 10;

        Dictionary<string, string> dataList = new Dictionary<string, string>();
        foreach (Node node in nodes) {
            if (node.nodeType == Node.NodeType.ConversationNode) {

                string conversationString = node.uniqueIDString;
                if (node.nodeData.ContainsKey("name")) {
                    if (node.nodeData["name"].Length != 0) conversationString = conversationString + " - " + node.nodeData["name"];
                }

                dataList.Add(conversationString, node.uniqueIDString);
            }
        }

        List<string> keyList = new List<string>();
        foreach (string key in dataList.Keys) keyList.Add(key);
        keyList.Sort();
        keyList.Insert(0, "-");

        GUILayout.Space(10);
        string selectedItem = keyList[EditorGUILayout.Popup(0, keyList.ToArray(), GUILayout.ExpandWidth(true))];
        if (selectedItem != "-") JumpToNode(dataList[selectedItem]);

    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor) {

        // Draws a grid over the entire window background
        Handles.BeginGUI();
        offset += drag * 0.5f;
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
        for (int i = 0; i < Mathf.CeilToInt(position.width / gridSpacing); i++) Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0), new Vector3(gridSpacing * i, position.height, 0f) + new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0));
        for (int j = 0; j < Mathf.CeilToInt(position.height / gridSpacing); j++) Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0), new Vector3(position.width, gridSpacing * j, 0f) + new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0));
        Handles.EndGUI();

    }

    private void ProcessEvents(Event currentEvent) {

        // Determine what will happen when the mouse it pressed
        drag = Vector2.zero;
        switch (currentEvent.type) {
            case EventType.MouseDown:
            
                if (currentEvent.button == 0) {

                    if (selectedOutPoint != null) ProcessContextMenu(currentEvent.mousePosition);
                    else ClearConnectionSelection();

                } else if (currentEvent.button == 1) ProcessContextMenu(currentEvent.mousePosition);
                break;

            case EventType.MouseDrag:

                if (currentEvent.button == 0) {
                    drag = currentEvent.delta;
                    if (nodes != null) {
                        for (int i = 0; i < nodes.Count; i++) {
                            nodes[i].DragAll(currentEvent.delta);
                            
                        }
                    }
                    GUI.changed = true;
                }

                break;

        }

    }

    private void DrawConnectionLine(Event currentEvent) {

        float curveLength = 100f;

        // Draw the white line that come up when the user is creating a connection between two nodes
        if (selectedInPoint != null && selectedOutPoint == null) {

            // Shrink the curve if it is getting too close to the other node
            float difference = selectedInPoint.rect.center.x - currentEvent.mousePosition.x;
            if (difference < curveLength) curveLength = difference;
            if (curveLength <= 0f) curveLength = 0f;

            Handles.DrawBezier(selectedInPoint.rect.center, currentEvent.mousePosition, selectedInPoint.rect.center + (Vector2.left * curveLength), currentEvent.mousePosition - (Vector2.left * curveLength), Color.white, null, 5f);
            GUI.changed = true;

        } else if (selectedOutPoint != null && selectedInPoint == null) {

            // Shrink the curve if it is getting too close to the other node
            float difference = currentEvent.mousePosition.x - selectedOutPoint.rect.center.x;
            if (difference < curveLength) curveLength = difference;
            if (curveLength <= 0f) curveLength = 0f;

            Handles.DrawBezier(selectedOutPoint.rect.center, currentEvent.mousePosition, selectedOutPoint.rect.center - (Vector2.left * curveLength), currentEvent.mousePosition + (Vector2.left * curveLength), Color.white, null, 5f);
            GUI.changed = true;

        }

    }

    public void ProcessContextMenu(Vector2 mousePosition) {

        // Let the user select what kind of node they want to add to the board
        GenericMenu genericMenu = new GenericMenu();

        if (selectedOutPoint != null) {
            genericMenu.AddItem(new GUIContent("Cancel"), false, () => ClearConnectionSelection());
            genericMenu.AddSeparator("");
        }

        if (selectedOutPoint == null) genericMenu.AddItem(new GUIContent("Begin Conversation"), false, () => OnClickAddNode(mousePosition, Node.NodeType.ConversationNode));
        genericMenu.AddItem(new GUIContent("End Conversation"), false, () => OnClickAddNode(mousePosition, Node.NodeType.EndNode));
        genericMenu.AddSeparator("");
        genericMenu.AddItem(new GUIContent("Dialogue"), false, () => OnClickAddNode(mousePosition, Node.NodeType.DialogueNode));
        genericMenu.AddItem(new GUIContent("Multiple Choice"), false, () => OnClickAddNode(mousePosition, Node.NodeType.MultipleChoiceNode));
        genericMenu.AddSeparator("");
        genericMenu.AddItem(new GUIContent("Random"), false, () => OnClickAddNode(mousePosition, Node.NodeType.RandomNode));
        genericMenu.AddItem(new GUIContent("Broadcast"), false, () => OnClickAddNode(mousePosition, Node.NodeType.BroadcastNode));
        genericMenu.AddSeparator("");
        genericMenu.AddItem(new GUIContent("Set Boolean"), false, () => OnClickAddNode(mousePosition, Node.NodeType.SetBooleanNode));
        genericMenu.AddItem(new GUIContent("Get Boolean"), false, () => OnClickAddNode(mousePosition, Node.NodeType.GetBooleanNode));
        if (selectedOutPoint == null) genericMenu.AddSeparator("");
        if (selectedOutPoint == null) genericMenu.AddItem(new GUIContent("Comment"), false, () => OnClickAddNode(mousePosition, Node.NodeType.CommentNode));
        genericMenu.ShowAsContext();

    }

    private void OnClickAddNode(Vector2 mousePosition, Node.NodeType nodeType) {

        Vector2 nodePos = mousePosition;
        if (selectedOutPoint != null) {
            Debug.Log((float)selectedOutPoint.parentNode.GetNodeInfo(nodeType)["width"]);
            nodePos = new Vector2(mousePosition.x + ((float)selectedOutPoint.parentNode.GetNodeInfo(nodeType)["width"]/2), mousePosition.y);
        }

        // Create a new node and add it to the list of exisiting nodes
        if (nodes == null) nodes = new List<Node>();
        Node newNode = new Node(nodePos, OnClickPoint, OnClickRemoveNode, nodeType, true);
        nodes.Add(newNode);


        if (selectedOutPoint != null) {
            selectedInPoint = newNode.inPoints[0];
            if (selectedOutPoint.parentNode != selectedInPoint.parentNode) {
                CreateConnection();
                ClearConnectionSelection();
            } else ClearConnectionSelection();

        }
        ClearConnectionSelection();

    }

    private void OnClickPoint(ConnectionPoint point, ConnectionPoint.ConnectionPointType connectionType) {

        // Determine if a connection should be made
        if (connectionType == ConnectionPoint.ConnectionPointType.In) {
            selectedInPoint = point;
            if (selectedOutPoint != null) {
                if (selectedOutPoint.parentNode != selectedInPoint.parentNode) {
                    CreateConnection();
                    ClearConnectionSelection();
                } else ClearConnectionSelection();
            }

        } else if (connectionType == ConnectionPoint.ConnectionPointType.Out) {
            selectedOutPoint = point;
            if (selectedInPoint != null) {
                if (selectedOutPoint.parentNode != selectedInPoint.parentNode) {
                    CreateConnection();
                    ClearConnectionSelection();
                } else ClearConnectionSelection();
            }

        }

    }

    private void OnClickRemoveNode(Node node) {

        // Remove any connections that are linked to this node
        if (connections != null) {
            List<Connection> connectionsToRemove = new List<Connection>();
            for (int i = 0; i < connections.Count; i++) if (node.inPoints.Contains(connections[i].inPoint) || node.outPoints.Contains(connections[i].outPoint)) connectionsToRemove.Add(connections[i]);
            for (int i = 0; i < connectionsToRemove.Count; i++) connections.Remove(connectionsToRemove[i]);
        }

        nodes.Remove(node);

    }

    private void OnClickRemoveConnection(Connection connection) {

        // Remove the desired connection
        connections.Remove(connection);

    }

    private void CreateConnection() {

        // Create the new node and draw it onto the board
        if (connections == null) connections = new List<Connection>();
        connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));

    }

    private void ClearConnectionSelection() {

        // Clear the connection variables
        selectedInPoint = null;
        selectedOutPoint = null;

    }

    private void SaveData() {

        // Turn all of the connections into a Serializable class
        List<JSONConnectionDictionary> JSONConnectDict = new List<JSONConnectionDictionary>();
        if (connections.Count != 0) {
            foreach (Connection currentConnection in connections) {
                JSONConnectionDictionary thisJSON = new JSONConnectionDictionary {
                    inPointID = currentConnection.inPoint.parentNode.nodeID,
                    outPointID = currentConnection.outPoint.parentNode.nodeID,
                    inOptionNumber = currentConnection.inPoint.optionNumber,
                    outOptionNumber = currentConnection.outPoint.optionNumber
                };
                JSONConnectDict.Add(thisJSON);
            }
        }

        // Save the data to a JSON file
        JSONList JSONListO = new JSONList {
            dataList = nodes,
            connectionList = JSONConnectDict
        };
        File.WriteAllText(filePath, JsonUtility.ToJson(JSONListO));

    }

    private void LoadData () {

        if (filePath.Length != 0) {

            // Clear any saved data and load in the file
            nodes = new List<Node>();
            connections = new List<Connection>();
            JSONList JSONList = JsonUtility.FromJson<JSONList>(File.ReadAllText(filePath));

            // Load in the node data
            foreach (Node currentDict in JSONList.dataList) {

                Node newNode = new Node(new Vector2(currentDict.actualNodeRect.position.x, currentDict.actualNodeRect.position.y), OnClickPoint, OnClickRemoveNode, currentDict.nodeType, false) {
                    nodeID = currentDict.nodeID,
                    uniqueIDString = currentDict.uniqueIDString,
                    
                };

                foreach (string key in currentDict.nodeData.Keys) {
                    newNode.nodeData[key] = currentDict.nodeData[key];
                }



                nodes.Add(newNode);
            }

            // Load in the connection data
            foreach (JSONConnectionDictionary currentDict in JSONList.connectionList) {

                ConnectionPoint inPoint = null;
                ConnectionPoint outPoint = null;

                foreach (Node currentNode in nodes) {

                    if (currentDict.inPointID == currentNode.nodeID) {
                        foreach (ConnectionPoint point in currentNode.inPoints) {
                            if (point.optionNumber == currentDict.inOptionNumber) {
                                inPoint = point;
                            }
                        }
                    }

                    if (currentDict.outPointID == currentNode.nodeID) {
                        foreach (ConnectionPoint point in currentNode.outPoints) {
                            if (point.optionNumber == currentDict.outOptionNumber) {
                                outPoint = point;
                            }
                        }
                    }

                }

                // Create the connection
                if (inPoint != null && outPoint != null) {
                    Connection newConnection = new Connection(inPoint, outPoint, OnClickRemoveConnection);
                    connections.Add(newConnection);
                }

            }

        }

    }

    public void ScrollToNode(float nodeX, float nodeY) {
        float windowCenterX = (position.width / 2) - nodeX;
        float windowCenterY = (position.height / 2) - nodeY;
        Vector2 nodePos = new Vector2(windowCenterX, windowCenterY);
        if (nodes != null) {
            for (int i = 0; i < nodes.Count; i++) {
                nodes[i].MoveTo(nodePos);

            }
        }
        GUI.changed = true;

    }

    public void JumpToNode(string nodeID) {

        foreach (Node currentNode in nodes) {
            if (currentNode.uniqueIDString == nodeID) ScrollToNode(currentNode.nodeRect.x + (currentNode.nodeRect.width/2), currentNode.nodeRect.y + (currentNode.nodeRect.height / 2));
        }

    }

}