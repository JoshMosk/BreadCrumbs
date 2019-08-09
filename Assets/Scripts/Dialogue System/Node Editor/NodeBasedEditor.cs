using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class NodeBasedEditor : EditorWindow {
    private List<Node> nodes = new List<Node>();
    private List<Connection> connections;

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 offset;
    private Vector2 drag;

    private string filePath = "";

    [MenuItem("Dialogue/Node Editor")]
    private static void OpenWindow() {

        NodeBasedEditor window = GetWindow<NodeBasedEditor>();
        window.titleContent = new GUIContent("Node Editor");
    }

    private void OnEnable() {
        LoadData();

        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);
    }

    private void OnGUI() {


        Texture2D tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        tex.SetPixel(0, 0, new Color(32 / 255f, 32 / 255f, 32 / 255f));
        tex.Apply();
        GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), tex, ScaleMode.StretchToFill);

        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        DrawConnections();
        DrawNodes();

        DrawConnectionLine(Event.current);

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        float insets = 20f;
        GUILayout.BeginArea(new Rect(insets, insets, 100, 150));

        if (GUILayout.Button("New")) {
            NewGameData();
        }

        if (GUILayout.Button("Save")) {
            SaveGameData();
        }

        if (GUILayout.Button("Save As")) {
            SaveAsGameData();
        }

        if (GUILayout.Button("Load")) {
            LoadGameData();
        }

        GUILayout.EndArea();

        if (GUI.changed) Repaint();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor) {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++) {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++) {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = new Color(91 / 255f, 169 / 255f, 193 / 255f);
        Handles.EndGUI();
    }

    private void DrawNodes() {
        if (nodes != null) {
            for (int i = 0; i < nodes.Count; i++) {
                nodes[i].Draw();
            }
        }
    }

    private void DrawConnections() {
        if (connections != null) {
            for (int i = 0; i < connections.Count; i++) {
                connections[i].Draw();
            }
        }
    }

    private void ProcessEvents(Event e) {
        drag = Vector2.zero;

        switch (e.type) {
            case EventType.MouseDown:
                if (e.button == 0) {
                    ClearConnectionSelection();
                    Debug.Log("New Ngode?");
                }

                if (e.button == 1) {
                    ProcessContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0) {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    private void ProcessNodeEvents(Event e) {
        if (nodes != null) {
            for (int i = nodes.Count - 1; i >= 0; i--) {
                bool guiChanged = nodes[i].ProcessEvents(e);

                if (guiChanged) {
                    GUI.changed = true;
                }
            }
        }
    }

    private void DrawConnectionLine(Event e) {
        if (selectedInPoint != null && selectedOutPoint == null) {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null) {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition) {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Conversation"), false, () => OnClickAddNode(mousePosition, "Conversation"));
        genericMenu.AddItem(new GUIContent("Dialogue"), false, () => OnClickAddNode(mousePosition, "Dialogue"));
        genericMenu.AddItem(new GUIContent("Multiple Choice"), false, () => OnClickAddNode(mousePosition, "Multiple Choice"));
        genericMenu.AddItem(new GUIContent("End Conversation"), false, () => OnClickAddNode(mousePosition, "End Conversation"));
        genericMenu.AddSeparator("");
        genericMenu.AddItem(new GUIContent("Random"), false, () => OnClickAddNode(mousePosition, "Random"));
        genericMenu.AddItem(new GUIContent("Boolean"), false, () => OnClickAddNode(mousePosition, "Boolean"));
        genericMenu.AddItem(new GUIContent("Set Variable"), false, () => OnClickAddNode(mousePosition, "Set Variable"));
        genericMenu.AddSeparator("");
        genericMenu.AddItem(new GUIContent("Comment"), false, () => OnClickAddNode(mousePosition, "Comment"));
        genericMenu.ShowAsContext();
    }

    private void OnDrag(Vector2 delta) {
        drag = delta;

        if (nodes != null) {
            for (int i = 0; i < nodes.Count; i++) {
                nodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void OnClickAddNode(Vector2 mousePosition, string typeString) {
        if (nodes == null) {
            nodes = new List<Node>();
        }

        nodes.Add(new Node(mousePosition, 200, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, typeString));
    }

    private void OnClickInPoint(ConnectionPoint inPoint) {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null) {
            if (selectedOutPoint.node != selectedInPoint.node) {
                CreateConnection();
                ClearConnectionSelection();
            } else {
                Debug.Log("New Node?");
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickOutPoint(ConnectionPoint outPoint) {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null) {
            if (selectedOutPoint.node != selectedInPoint.node) {
                CreateConnection();
                ClearConnectionSelection();
            } else {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickRemoveNode(Node node) {
        if (connections != null) {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < connections.Count; i++) {
                if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint) {
                    connectionsToRemove.Add(connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++) {
                connections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }

        nodes.Remove(node);
    }

    private void OnClickRemoveConnection(Connection connection) {
        connections.Remove(connection);
    }

    private void CreateConnection() {
        if (connections == null) {
            connections = new List<Connection>();
        }

        Debug.Log(selectedInPoint);
        connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }

    private void ClearConnectionSelection() {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    private void NewGameData() {

        nodes = new List<Node>();
        connections = new List<Connection>();
        filePath = "";

    }

    private void LoadGameData() {

        filePath = EditorUtility.OpenFilePanel("Overwrite with png", "Assets", "json");
        if (filePath.Length != 0) {
            LoadData();
        }

    }

    private void LoadData() {

        if (filePath.Length != 0) {
            nodes = new List<Node>();
            connections = new List<Connection>();

            var fileContent = File.ReadAllBytes(filePath);
            string dataAsJson = File.ReadAllText(filePath);
            JSONList JSONListO = JsonUtility.FromJson<JSONList>(dataAsJson);

            foreach (JSONDictionary currentDict in JSONListO.dataList) {
                Node newNode = new Node(new Vector2(currentDict.position.x, currentDict.position.y), 200, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, currentDict.type);
                newNode.nodeID = currentDict.nodeID;
                newNode.titleString = currentDict.name;
                newNode.bodyString = currentDict.body;
                newNode.uniqueIDString = currentDict.uniqueIDString;
                newNode.blackboardString = currentDict.blackboardString;
                newNode.valueBool = currentDict.variableValue;
                newNode.variableString = currentDict.variableString;
                newNode.type = currentDict.type;
                newNode.option1String = currentDict.option1;
                newNode.option2String = currentDict.option2;
                newNode.option3String = currentDict.option3;
                newNode.option4String = currentDict.option4;
                nodes.Add(newNode);
            }

            foreach (JSONConnectionDictionary currentDict in JSONListO.connectionList) {

                ConnectionPoint inPoint = null;
                ConnectionPoint outPoint = null;


                foreach (Node currentNode in nodes) {
                    if (currentDict.inPointID == currentNode.nodeID) {
                        inPoint = currentNode.inPoint;
                    }

                    if (currentDict.outPointID == currentNode.nodeID) {
                        if (currentNode.type == "Boolean") {
                            if (currentDict.outBoolType) {
                                outPoint = currentNode.outPoint;
                            } else {
                                outPoint = currentNode.outPoint2;
                            }
                        } else if (currentNode.type == "Multiple Choice") {
                            if (currentDict.optionNumber == 1) outPoint = currentNode.outPoint;
                            else if (currentDict.optionNumber == 2) outPoint = currentNode.outPoint2;
                            else if (currentDict.optionNumber == 3) outPoint = currentNode.outPoint3;
                            else if (currentDict.optionNumber == 4) outPoint = currentNode.outPoint4;


                        } else {
                            outPoint = currentNode.outPoint;
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

    private void SaveGameData() {
        if (filePath.Length == 0) filePath = EditorUtility.SaveFilePanel("Save As...", "Assets", "Untitled" + ".json", "json");
        SaveData();
    }

    private void SaveAsGameData() {
        filePath = EditorUtility.SaveFilePanel("Save As...", "Assets", "Untitled" + ".json", "json");
        SaveData();
    }

    private void SaveData() {
        List<JSONDictionary> JSONDict = new List<JSONDictionary>();
        foreach (Node currentNode in nodes) {
            JSONDictionary thisJSON = new JSONDictionary();
            thisJSON.nodeID = currentNode.nodeID;
            thisJSON.name = currentNode.titleString;
            thisJSON.body = currentNode.bodyString;
            thisJSON.uniqueIDString = currentNode.uniqueIDString;
            thisJSON.blackboardString = currentNode.blackboardString;
            thisJSON.variableValue = currentNode.valueBool;
            thisJSON.variableString = currentNode.variableString;
            thisJSON.position = currentNode.rect;
            thisJSON.type = currentNode.type;
            thisJSON.option1 = currentNode.option1String;
            thisJSON.option2 = currentNode.option2String;
            thisJSON.option3 = currentNode.option3String;
            thisJSON.option4 = currentNode.option4String;
            JSONDict.Add(thisJSON);
        }

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

        JSONList JSONListO = new JSONList();
        JSONListO.dataList = JSONDict;
        JSONListO.connectionList = JSONConnectDict;

        string dataAsJson = JsonUtility.ToJson(JSONListO);
        File.WriteAllText(filePath, dataAsJson);

    }
}