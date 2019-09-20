using System.Collections;
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

    [Header("Blackboards")]
    public string fileName;
    public Dictionary<string, Dictionary<string, bool>> blackboards = new Dictionary<string, Dictionary<string, bool>>();

    [Header("User Interface")]
    public Text speakerTextBox;
    public Text bodyTextBox;
    public GameObject panelObject;

    [Header("Multiple Choice")]
    public Button option1Button;
    public Button option2Button;
    public Button option3Button;
    public Button option4Button;
    public GameObject indicatorArrow;


    private int selectedOption;
    public string conversationID;
    public Transform speakerObject;

    private bool canSkip = true;
    private bool isTyping = false;
    public bool inConversation = false;

    private Node currentNode;
    private JSONList loadedData;

    Coroutine currentEnumerator;

    public IInput m_input;

    public bool isWorldSpace;

    public GameObject startPanel;

    public float cooldownTimer;


    private void Start() {

        // Set up the multiple choice buttons
        option1Button.onClick.AddListener(delegate { SelectOption(1); });
        option2Button.onClick.AddListener(delegate { SelectOption(2); });
        option3Button.onClick.AddListener(delegate { SelectOption(3); });
        option4Button.onClick.AddListener(delegate { SelectOption(4); });
        HideMultipleChoice();

        m_input = GetComponent<IInput>();

    }

    private void Update() {

        DialogueManager.instance.cooldownTimer -= Time.deltaTime;

        if (inConversation) {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.01124641f, 0.01124641f, 0.01124641f), 3f * Time.deltaTime);
        } else {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 0), 3f * Time.deltaTime);
        }

        if (isWorldSpace) {
            transform.LookAt(Camera.main.transform, Vector3.up);
          //  transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(0, transform.localEulerAngles.y + 180, 0), .5f * Time.deltaTime);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 180, 0);
            if (speakerObject) {
                // transform.position = Vector3.Lerp(transform.position, , .5f * Time.deltaTime);
                transform.position = new Vector3(speakerObject.transform.position.x, speakerObject.transform.position.y + 6f, speakerObject.transform.position.z);
            }
        }

        // Show and hide the dialogue box
        if (inConversation) {
            panelObject.GetComponent<CanvasGroup>().alpha = 1f;

            // Determine if the user can go to the next dialogue in the conversation
            if (m_input.NPCInteractDown || Input.GetKeyDown(KeyCode.Tab)) {
                    if (isTyping) isTyping = false;
                else if (canSkip) FindNextNode();
            }

        } else panelObject.GetComponent<CanvasGroup>().alpha = 0f;

    }


    public void SetBlackboardVariable(string blackboard, string variable, bool value) {

        if (blackboards.ContainsKey(blackboard)) {

            // Check if the blackboard alreavy has the variable as a key
            if (blackboards[blackboard].ContainsKey(variable)) blackboards[blackboard][variable] = value;
            else blackboards[blackboard].Add(variable, value);

        } else {

            // There is no stored data, set it up as new
            Dictionary<string, bool> newDict = new Dictionary<string, bool> {
                { variable, value }
            };
            blackboards.Add(blackboard, newDict);

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

        loadedData = new JSONList {
            dataList = nodes,
            connectionList = JSONConnectDict
        };

     //   loadedData = JsonUtility.FromJson<JSONList>(File.ReadAllText(Application.streamingAssetsPath + "/" + fileName + ".json"));

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

            if (currentNode.nodeData["event"] == "") {

            }

            FindNextNode();


        } else if (currentNode.nodeType == Node.NodeType.DialogueNode) {
            currentEnumerator = StartCoroutine(TypeText((string)currentNode.nodeData["speaker"], (string)currentNode.nodeData["dialogue"]));
          //  foreach (DialogueCharacter currentCharacter in FindObjectsOfType<DialogueCharacter>()) if (currentCharacter.name == (string)currentNode.nodeData["speaker"]) speakerObject = currentCharacter.gameObject.transform;

        } else if (currentNode.nodeType == Node.NodeType.MultipleChoiceNode) {
            canSkip = false;
            currentEnumerator = StartCoroutine(TypeText((string)currentNode.nodeData["speaker"], (string)currentNode.nodeData["dialogue"]));
          //  foreach (DialogueCharacter currentCharacter in FindObjectsOfType<DialogueCharacter>()) if (currentCharacter.name == (string)currentNode.nodeData["speaker"]) speakerObject = currentCharacter.gameObject.transform;

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


        }
        else if (currentNode.nodeType == Node.NodeType.GetBooleanNode)
        {
            bool blackboardBool = GetBlackboardVariable(currentNode.nodeData["blackboard"], currentNode.nodeData["variable"]);
            if (blackboardBool) selectedOption = 1;
            else selectedOption = 2;
            FindNextNode();


        }
        else if (currentNode.nodeType == Node.NodeType.EndNode) {
            cooldownTimer = 1f;
            inConversation = false;
            DialolgueIcon.instance.SetVisible(true);


        }
    }

    IEnumerator TypeText(string characterName, string bodyString) {

        isTyping = true;
        bodyTextBox.text = bodyString;

        if (characterName.Contains("Shadow")) {
            speakerTextBox.text = "Shadow Creature";
        }

        speakerTextBox.text = characterName;

        TextGenerator speakerTextGen = new TextGenerator();
        TextGenerationSettings speakerGenerationSettings = speakerTextBox.GetGenerationSettings(speakerTextBox.rectTransform.rect.size);

        TextGenerator bodyTextGen = new TextGenerator();
        TextGenerationSettings bodyGenerationSettings = bodyTextBox.GetGenerationSettings(bodyTextBox.rectTransform.rect.size);

        float speakerHeight = speakerTextGen.GetPreferredWidth(characterName, speakerGenerationSettings);
        float bodyHeight = bodyTextGen.GetPreferredHeight(bodyString, bodyGenerationSettings);

        Debug.Log("Text size " + (speakerHeight + bodyHeight));


        // Temp lets just not have typed letters rn

        /*
        bodyTextBox.text = "";
        bodyString = bodyString.Replace("…", "...");

        foreach (char letter in bodyString) {

            if (isTyping == false) bodyTextBox.text = bodyString;
            else {

                bodyTextBox.text += letter;
                if (letter == ".".ToCharArray()[0]) yield return new WaitForSeconds(.2f);
                else if (letter == ",".ToCharArray()[0]) yield return new WaitForSeconds(.1f);
                else if (letter == "!".ToCharArray()[0]) yield return new WaitForSeconds(.1f);
                else if (letter == "?".ToCharArray()[0]) yield return new WaitForSeconds(.1f);
                else yield return new WaitForSeconds(.02f);

            }

        }

    */

        isTyping = false;
        yield return new WaitForSeconds(.0f);
    }

    void ShowMultipleChoice(int optionCount) {

        if (optionCount >= 1) option1Button.gameObject.SetActive(true);
        if (optionCount >= 2) option2Button.gameObject.SetActive(true);
        if (optionCount >= 3) option3Button.gameObject.SetActive(true);
        if (optionCount >= 4) option4Button.gameObject.SetActive(true);
        indicatorArrow.SetActive(true);

       // option1Button.gameObject.transform.localPosition = new Vector3(730, 170 + (90 * (optionCount - 1)), 0);
       // option2Button.gameObject.transform.localPosition = new Vector3(730, 170 + (90 * (optionCount - 2)), 0);
       // option3Button.gameObject.transform.localPosition = new Vector3(730, 170 + (90 * (optionCount - 3)), 0);
       // option4Button.gameObject.transform.localPosition = new Vector3(730, 170 + (90 * (optionCount - 4)), 0);

    }

    void HideMultipleChoice() {

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