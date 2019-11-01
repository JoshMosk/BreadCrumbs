﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager instance;
    void Awake() {
        if (instance != null) return;
        instance = this;
    }

    [Header("Conversation")]
    public string conversationID;
    public Transform speakerObject;
    public Dictionary<string, Dictionary<string, bool>> blackboards = new Dictionary<string, Dictionary<string, bool>>();

    [Header("Interface")]
    public Text speakerTextBox;
    public Text bodyTextBox;
    public GameObject panelObject;
    private CanvasGroup canvGroup;

    [Header("Input")]
    public IInput m_input;
    private int selectedOption;
    public Button option1Button;
    public Button option2Button;
    public Button option3Button;
    public Button option4Button;
    public GameObject indicatorArrow;

    [Header("Puzzles")]
    public PuzzleComplete puzzle1Completed;
    public PuzzleComplete puzzle2Completed;
    public PuzzleComplete puzzle3Completed;

    [Header("Status")]
    public bool canSkip = true;
    public bool isTyping = false;
    public bool isWorldSpace = true;
    public bool inConversation = false;
    public bool isMultipleChoice = false;

    private Node currentNode;
    private JSONList loadedData;
    private Coroutine currentEnumerator;
    [HideInInspector] public float cooldownTimer;
    [HideInInspector] public float cooldownTimerNextNode;

    private void Start() {

        // Set up the multiple choice buttons
        option1Button.onClick.AddListener(delegate { SelectOption(1); });
        option2Button.onClick.AddListener(delegate { SelectOption(2); });
        option3Button.onClick.AddListener(delegate { SelectOption(3); });
        option4Button.onClick.AddListener(delegate { SelectOption(4); });
        HideMultipleChoice();

        m_input = GetComponent<IInput>();
        canvGroup = panelObject.GetComponent<CanvasGroup>();
    }

    private void Update() {

        cooldownTimer -= Time.deltaTime;
        cooldownTimerNextNode -= Time.deltaTime;

        if (inConversation) transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.01124641f, 0.01124641f, 0.01124641f), 1f * Time.deltaTime);
        else transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 0), 1f * Time.deltaTime);

        if (isWorldSpace) {
            transform.LookAt(Camera.main.transform, Vector3.up);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 180, 0);
            if (speakerObject) {
                transform.position = new Vector3(speakerObject.transform.position.x, speakerObject.transform.position.y + 6f, speakerObject.transform.position.z);
            }
        }

        // Show and hide the dialogue box
        if (inConversation) {
            canvGroup.alpha = 1f;

            // Determine if the user can go to the next dialogue in the conversation
            if (m_input.NPCInteractDown || Input.GetKeyDown(KeyCode.Tab)) {
                if (cooldownTimerNextNode <= 0f) {
                    cooldownTimerNextNode = 1f;

                    // if (isTyping) isTyping = false;
                    // else if (canSkip)
                    FindNextNode();
                    
                }
            }

        } else canvGroup.alpha = 0f;

    }

    public void SetBlackboardVariable(string blackboard, string variable, bool value) {

        if (blackboards.ContainsKey(blackboard)) {

            // Check if the blackboard alreavy has the variable as a key
            if (blackboards[blackboard].ContainsKey(variable)) blackboards[blackboard][variable] = value;
            else blackboards[blackboard].Add(variable, value);

        } else {

            // There is no stored data, set it up as new
            blackboards.Add(blackboard, new Dictionary<string, bool> { { variable, value } });

        }

    }

    public bool GetBlackboardVariable(string blackboard, string variable) {

        // Return the variable that was requested
        if (blackboards.ContainsKey(blackboard)) {
            if (blackboards[blackboard].ContainsKey(variable)) return blackboards[blackboard][variable];
        }

        return false;

    }


    public void StartConversation(string sentConversationID) {

        DialolgueIcon.instance.SetVisible(false);

        if (currentEnumerator != null) StopCoroutine(currentEnumerator);

        // Load in the conversation data
        inConversation = true;
        conversationID = sentConversationID;

        List<Node> nodes = new List<Node>();
        List<JSONConnectionDictionary> JSONConnectDict = new List<JSONConnectionDictionary>();

        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info) {

            if (f.Name.EndsWith(".json")) {
                JSONList thisData = JsonUtility.FromJson<JSONList>(File.ReadAllText(Application.streamingAssetsPath + "/" + f.Name));
                nodes.AddRange(thisData.dataList);
                JSONConnectDict.AddRange(thisData.connectionList);
            }
        }

        loadedData = new JSONList {dataList = nodes, connectionList = JSONConnectDict};

        // Loop through each node in the file to find the conversation
        foreach (Node currentDict in loadedData.dataList) {

            if (conversationID == currentDict.uniqueIDString) {
                currentNode = currentDict;
                selectedOption = 1;
                FindNextNode();
            }
        }

    }

    private void FindNextNode() {

        // Find the connection that leads away from this current node
        foreach (JSONConnectionDictionary currentLink in loadedData.connectionList) {
            if (currentLink.outPointID == currentNode.nodeID && currentLink.outOptionNumber == selectedOption) {

                foreach (Node currentDict in loadedData.dataList) {
                    if (currentDict.nodeID == currentLink.inPointID) {
                        selectedOption = 1;
                        currentNode = currentDict;
                        ProcessNode();
                        return;

                    }
                }

            }
        }

    }

    private void ProcessNode() {

        if (currentNode.nodeType == Node.NodeType.BroadcastNode) {

            if (currentNode.nodeData["event"] == "Puzzle1Complete") puzzle1Completed.CompletePuzzle();
			if (currentNode.nodeData["event"] == "Puzzle2Complete") puzzle2Completed.CompletePuzzle();
            if (currentNode.nodeData["event"] == "Puzzle3Complete") puzzle3Completed.CompletePuzzle();
			FindNextNode();

        } else if (currentNode.nodeType == Node.NodeType.DialogueNode) {
            currentEnumerator = StartCoroutine(TypeText((string)currentNode.nodeData["speaker"], (string)currentNode.nodeData["dialogue"]));

        } else if (currentNode.nodeType == Node.NodeType.MultipleChoiceNode) {
            canSkip = false;
            currentEnumerator = StartCoroutine(TypeText((string)currentNode.nodeData["speaker"], (string)currentNode.nodeData["dialogue"]));

            int optionNumbers = 0;
            if (currentNode.nodeData["option1"].Length != 0) optionNumbers++;
            if (currentNode.nodeData["option2"].Length != 0) optionNumbers++;
            if (currentNode.nodeData["option3"].Length != 0) optionNumbers++;
            if (currentNode.nodeData["option4"].Length != 0) optionNumbers++;
            ShowMultipleChoice(optionNumbers);

            option1Button.GetComponentInChildren<Text>().text = (string)currentNode.nodeData["option1"];
            option2Button.GetComponentInChildren<Text>().text = (string)currentNode.nodeData["option2"];
            option3Button.GetComponentInChildren<Text>().text = (string)currentNode.nodeData["option3"];
            option4Button.GetComponentInChildren<Text>().text = (string)currentNode.nodeData["option4"];

        } else if (currentNode.nodeType == Node.NodeType.SetBooleanNode) {
            instance.SetBlackboardVariable((string)currentNode.nodeData["blackboard"], (string)currentNode.nodeData["variable"], Node.BoolFromString(currentNode.nodeData["value"]));
            FindNextNode();

        } else if (currentNode.nodeType == Node.NodeType.RandomNode) {
            List<string> inNodes = new List<string>();
            foreach (JSONConnectionDictionary currentRandom in loadedData.connectionList) if (currentRandom.outPointID == currentNode.nodeID) inNodes.Add(currentRandom.inPointID);

            string randomStr = inNodes[Random.Range(0, inNodes.Count)];
            foreach (Node loopNode in loadedData.dataList) {
                if (loopNode.nodeID == randomStr) {
                    currentNode = loopNode;
                    ProcessNode();
                }
            }

        } else if (currentNode.nodeType == Node.NodeType.GetBooleanNode) {
            bool blackboardBool = GetBlackboardVariable(currentNode.nodeData["blackboard"], currentNode.nodeData["variable"]);
            if (blackboardBool) selectedOption = 1;
            else selectedOption = 2;
            FindNextNode();


        } else if (currentNode.nodeType == Node.NodeType.EndNode) {
            cooldownTimer = 1f;
            inConversation = false;
            DialolgueIcon.instance.SetVisible(true);

        }
    }

    IEnumerator TypeText(string characterName, string bodyString) {

		isTyping = true;
        bodyTextBox.text = bodyString;

        if (characterName.Contains("NPC") || characterName.Contains("Shadow Creature")) {
            characterName = "Shadow Creature";
        }

        speakerTextBox.text = characterName;

        /*
        bodyTextBox.text = "";
        bodyString = bodyString.Replace("…", "...");

        foreach (char letter in bodyString) {

            if (isTyping == false) bodyTextBox.text = bodyString;
            else {

                bodyTextBox.text += letter;
                if (letter == ".".ToCharArray()[0]) yield return new WaitForSeconds(.1f);
                else if (letter == ",".ToCharArray()[0]) yield return new WaitForSeconds(.05f);
                else if (letter == "!".ToCharArray()[0]) yield return new WaitForSeconds(.05f);
                else if (letter == "?".ToCharArray()[0]) yield return new WaitForSeconds(.5f);
                else yield return new WaitForSeconds(.01f);

            }

        }

    */

        isTyping = false;
        yield return new WaitForSeconds(.01f);
    }

    void ShowMultipleChoice(int optionCount) {
        isMultipleChoice = true;
        if (optionCount >= 1) option1Button.gameObject.SetActive(true);
        if (optionCount >= 2) option2Button.gameObject.SetActive(true);
        if (optionCount >= 3) option3Button.gameObject.SetActive(true);
        if (optionCount >= 4) option4Button.gameObject.SetActive(true);
        indicatorArrow.SetActive(true);

    }

    void HideMultipleChoice() {
        isMultipleChoice = false;
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);
        option3Button.gameObject.SetActive(false);
        option4Button.gameObject.SetActive(false);
        indicatorArrow.SetActive(false);

    }

    void SelectOption(int index) {
        isTyping = false;
        if (currentEnumerator != null) StopCoroutine(currentEnumerator);
        selectedOption = index;
        canSkip = true;
        HideMultipleChoice();
        FindNextNode();

    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos) {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }

}