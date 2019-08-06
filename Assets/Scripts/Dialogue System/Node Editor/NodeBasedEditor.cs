using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class NodeBasedEditor : EditorWindow {

    private List<Connection> connections;
    private List<Node> nodes = new List<Node>();

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 drag;
    private Vector2 offset;

    private string filePath = "";


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

        if (GetWindow<NodeBasedEditor>().filePath.Length == 0) GetWindow<NodeBasedEditor>().filePath = EditorUtility.SaveFilePanel("Save As...", "Assets/StreamingAssets", "Untitled" + ".json", "json");
        GetWindow<NodeBasedEditor>().SaveData();

    }

    [MenuItem("Dialogue/Save As...", false, 200)]
    public static void SaveAsFile() {
        GetWindow<NodeBasedEditor>().filePath = EditorUtility.SaveFilePanel("Save As...", "Assets/StreamingAssets", "Untitled" + ".json", "json");
        GetWindow<NodeBasedEditor>().SaveData();
    }



    private void OnEnable() {

        // Load in the data when the window is opened
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

        // Draw all of the connections and nodes
        if (connections != null) for (int i = 0; i < connections.Count; i++) connections[i].Draw();
        if (nodes != null) for (int i = 0; i < nodes.Count; i++) nodes[i].Draw();

        DrawConnectionLine(Event.current);

        // Process events for when the user interacts with the window
        if (nodes != null) for (int i = nodes.Count - 1; i >= 0; i--) GUI.changed = nodes[i].ProcessEvents(Event.current);
        ProcessEvents(Event.current);

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
            
                if (currentEvent.button == 0) ClearConnectionSelection();
                else if (currentEvent.button == 1) ProcessContextMenu(currentEvent.mousePosition);
                break;

            case EventType.MouseDrag:

                if (currentEvent.button == 0) {
                    drag = currentEvent.delta;
                    if (nodes != null) for (int i = 0; i < nodes.Count; i++) nodes[i].Drag(currentEvent.delta);
                    GUI.changed = true;
                }

                break;

        }

    }

    private void DrawConnectionLine(Event currentEvent) {

        // Draw the white line that come up when the user is creating a connection between two nodes
        if (selectedInPoint != null && selectedOutPoint == null) {
            Handles.DrawBezier(selectedInPoint.rect.center, currentEvent.mousePosition, selectedInPoint.rect.center + Vector2.left * 100f, currentEvent.mousePosition - Vector2.left * 100f, Color.white, null, 6f);
            GUI.changed = true;

        } else if (selectedOutPoint != null && selectedInPoint == null) {
            Handles.DrawBezier(selectedOutPoint.rect.center, currentEvent.mousePosition, selectedOutPoint.rect.center - Vector2.left * 100f, currentEvent.mousePosition + Vector2.left * 100f, Color.white, null, 6f);
            GUI.changed = true;

        }
    }

    private void ProcessContextMenu(Vector2 mousePosition) {

       // Node.

        // Let the user select what kind of node they want to add to the board
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent(""), false, () => OnClickAddNode(mousePosition, Node.NodeType.ConversationNode));
        genericMenu.AddItem(new GUIContent("End Conversation"), false, () => OnClickAddNode(mousePosition, Node.NodeType.EndNode));
        genericMenu.AddSeparator("");
        genericMenu.AddItem(new GUIContent("Dialogue"), false, () => OnClickAddNode(mousePosition, Node.NodeType.DialogueNode));
        genericMenu.AddItem(new GUIContent("Multiple Choice"), false, () => OnClickAddNode(mousePosition, Node.NodeType.MultipleChoiceNode));
        genericMenu.AddSeparator("");
        genericMenu.AddItem(new GUIContent("Random"), false, () => OnClickAddNode(mousePosition, Node.NodeType.RandomNode));
        genericMenu.AddItem(new GUIContent("Set Boolean"), false, () => OnClickAddNode(mousePosition, Node.NodeType.SetBooleanNode));
        genericMenu.AddItem(new GUIContent("Get Boolean"), false, () => OnClickAddNode(mousePosition, Node.NodeType.GetBooleanNode));
        genericMenu.AddSeparator("");
        genericMenu.AddItem(new GUIContent("Comment"), false, () => OnClickAddNode(mousePosition, Node.NodeType.CommentNode));
        genericMenu.ShowAsContext();

    }

    private void OnClickAddNode(Vector2 mousePosition, Node.NodeType nodeType) {

        // Create a new node and add it to the list of exisiting nodes
        if (nodes == null) nodes = new List<Node>();
        nodes.Add(new Node(mousePosition, OnClickPoint, OnClickRemoveNode, nodeType));

    }

    private void OnClickPoint(ConnectionPoint point, ConnectionPointType connectionType) {

        // Determine if a connection should be made
        if (connectionType == ConnectionPointType.In) {
            selectedInPoint = point;
            if (selectedOutPoint != null) {
                if (selectedOutPoint.node != selectedInPoint.node) {
                    CreateConnection();
                    ClearConnectionSelection();
                } else ClearConnectionSelection();
            }

        } else if (connectionType == ConnectionPointType.Out) {
            selectedOutPoint = point;
            if (selectedInPoint != null) {
                if (selectedOutPoint.node != selectedInPoint.node) {
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
                JSONConnectionDictionary thisJSON = new JSONConnectionDictionary();
                thisJSON.inPointID = currentConnection.inPoint.node.nodeID;
                thisJSON.outPointID = currentConnection.outPoint.node.nodeID;
                thisJSON.outBoolType = currentConnection.outPoint.boolType;
                thisJSON.optionNumber = currentConnection.outPoint.optionNumber;
                JSONConnectDict.Add(thisJSON);
            }
        }

        // Save the data to a JSON file
        JSONList JSONListO = new JSONList();
        JSONListO.dataList = nodes;
        JSONListO.connectionList = JSONConnectDict;
        File.WriteAllText(filePath, JsonUtility.ToJson(JSONListO));

    }














    private void LoadData () {

        if (filePath.Length != 0) {
            nodes = new List<Node>();
            connections = new List<Connection>();
            JSONList JSONList = JsonUtility.FromJson<JSONList>(File.ReadAllText(filePath));

            foreach (Node currentDict in JSONList.dataList) {
                Node newNode = new Node(new Vector2(currentDict.rect.position.x, currentDict.rect.position.y), OnClickPoint, OnClickRemoveNode, currentDict.type);
                newNode.nodeID = currentDict.nodeID;
                newNode.titleString = currentDict.titleString;
                newNode.bodyString = currentDict.bodyString;
                newNode.uniqueIDString = currentDict.uniqueIDString;
                newNode.blackboardString = currentDict.blackboardString;
                newNode.valueBool = currentDict.valueBool;
                newNode.variableString = currentDict.variableString;
                newNode.type = currentDict.type;
                newNode.option1String = currentDict.option1String;
                newNode.option2String = currentDict.option2String;
                newNode.option3String = currentDict.option3String;
                newNode.option4String = currentDict.option4String;
                nodes.Add(newNode);
            }

            foreach (JSONConnectionDictionary currentDict in JSONList.connectionList) {

                ConnectionPoint inPoint = null;
                ConnectionPoint outPoint = null;


                foreach (Node currentNode in nodes) {
                    if (currentDict.inPointID == currentNode.nodeID) {
                        //inPoint = currentNode.inPoint;
                    }

                    if (currentDict.outPointID == currentNode.nodeID) {
                        if (currentNode.type == Node.NodeType.GetBooleanNode) {
                            if (currentDict.outBoolType) {
                                //outPoint = currentNode.outPoint;
                            } else {
                                //outPoint = currentNode.outPoint2;
                            }
                        }
                        else if (currentNode.type == Node.NodeType.MultipleChoiceNode)
                        {
                            //if (currentDict.optionNumber == 1) outPoint = currentNode.outPoint;
                            //else if (currentDict.optionNumber == 2) outPoint = currentNode.outPoint2;
                            //else if (currentDict.optionNumber == 3) outPoint = currentNode.outPoint3;
                            //else if (currentDict.optionNumber == 4) outPoint = currentNode.outPoint4;


                        }




                        else {
                            //outPoint = currentNode.outPoint;
                        }

                    }                 
                }

                if (inPoint != null && outPoint != null) {
                    Connection newConnection = new Connection(inPoint, outPoint, OnClickRemoveConnection);
                    connections.Add(newConnection);
                }


            }
        }

    }

}