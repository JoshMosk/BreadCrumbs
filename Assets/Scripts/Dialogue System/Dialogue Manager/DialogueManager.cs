using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of DialogueManager found");
            return;
        }
        instance = this;
    }

    [Header("Blackboards")]
    public string currentChapter;
    public Dictionary<string, Dictionary<string, bool>> blackboards = new Dictionary<string, Dictionary<string, bool>>();

    [Header("User Interface")]
    public Text speakerTextBox;
    public Text bodyTextBox;
    public GameObject panelObject;

    // Dialogue Details
    private Transform currentSpeaker;
    private bool inConversation = false;
    private string currentConversationID;

    // Loaded Dialogue Data
    private JSONList currentJSONList;
    private JSONDictionary currentDictionary;

    // Multiple Choice
    public bool isMultipleChoice;
    int selectedOption;
    public Button multipleChoiceOne;
    public Button multipleChoiceTwo;
    public Button multipleChoiceThree;
    public Button multipleChoiceFour;

    public enum TypeStatus
    {
        Typing,
        Complete,
        Waiting,
    }

    public TypeStatus typeStatus;



    #region Setup

    private void Start()
    {
        multipleChoiceOne.onClick.AddListener(PressOne);
        multipleChoiceTwo.onClick.AddListener(PressTwo);
        multipleChoiceThree.onClick.AddListener(PressThree);
        multipleChoiceFour.onClick.AddListener(PressFour);
        HideButtons();
    }

    private void Update()
    {
        if (inConversation)
        {
            panelObject.GetComponent<CanvasGroup>().alpha = 1f;
            if (Input.GetKeyDown(KeyCode.Space) && !isMultipleChoice)
            {
                if (typeStatus == TypeStatus.Typing) typeStatus = TypeStatus.Complete;
                else FindNextNode();
            }

        }
        else panelObject.GetComponent<CanvasGroup>().alpha = 0f;
    }

    #endregion


    #region Blackboards
    public void SetBlackboardVariable(string blackboard, string variable, bool value)
    {

        if (blackboards.ContainsKey(blackboard))
        {

            if (blackboards[blackboard].ContainsKey(variable))
            {

                // Has Blackboard and Variable
                blackboards[blackboard][variable] = value;

            }
            else
            {
                // Has Blackboard, but not Variable
                blackboards[blackboard].Add(variable, value);
            }

        }
        else
        {
            // No Blackboard or Variable

            Dictionary<string, bool> newDict = new Dictionary<string, bool>();
            newDict.Add(variable, value);
            blackboards.Add(blackboard, newDict);
        }
    }

    public bool GetBlackboardVariable(string blackboard, string variable)
    {

        if (blackboards.ContainsKey(blackboard))
        {
            if (blackboards[blackboard].ContainsKey(variable)) return blackboards[blackboard][variable];
            else return false;

        }
        else return false;
    }

    #endregion


    #region Dialogue
    public void StartConversation(string conversationID)
    {

        // Load in the conversation data.
        inConversation = true;
        currentConversationID = conversationID;
        currentJSONList = JsonUtility.FromJson<JSONList>(File.ReadAllText(Application.streamingAssetsPath + "/Dialogue/" + currentChapter + ".json"));

        // Loop through each node in the conversation.
        foreach (JSONDictionary currentDict in currentJSONList.dataList)
        {
            if (currentConversationID == currentDict.uniqueIDString)
            {
                currentDictionary = currentDict;
                FindNextNode();
            }
        }
    }

    private void FindNextNode()
    {
        foreach (JSONConnectionDictionary currentLink in currentJSONList.connectionList)
        {
            if (currentLink.outPointID == currentDictionary.nodeID)
            {
                if (currentLink.outPointID == currentDictionary.nodeID)
                {
                    foreach (JSONDictionary currentDict in currentJSONList.dataList)
                    {
                        if (currentDict.nodeID == currentLink.inPointID)
                        {

                            currentDictionary = currentDict;
                            ProcessNode();
                            return;

                        }
                    }
                }
            }
        }
    }

    private void ProcessNode()
    {
        if (currentDictionary.type == "Dialogue")
        {
            //  speakerTextBox.text = currentDictionary.name;
            // bodyTextBox.text = currentDictionary.body;


            StartCoroutine(TypeText(currentDictionary.name, currentDictionary.body));

            foreach (DialogueCharacter currentCharacter in FindObjectsOfType<DialogueCharacter>())
            {
                if (currentCharacter.characterName == currentDictionary.name) currentSpeaker = currentCharacter.gameObject.transform;
            }

        }
        else if (currentDictionary.type == "Multiple Choice")
        {
            isMultipleChoice = true;
            StartCoroutine(TypeText(currentDictionary.name, currentDictionary.body));

            foreach (DialogueCharacter currentCharacter in FindObjectsOfType<DialogueCharacter>())
            {
                if (currentCharacter.characterName == currentDictionary.name) currentSpeaker = currentCharacter.gameObject.transform;
            }

            multipleChoiceOne.gameObject.SetActive(true);
            multipleChoiceTwo.gameObject.SetActive(true);
            multipleChoiceThree.gameObject.SetActive(true);
            multipleChoiceFour.gameObject.SetActive(true);

            multipleChoiceOne.GetComponentInChildren<Text>().text = currentDictionary.option1;
            multipleChoiceTwo.GetComponentInChildren<Text>().text = currentDictionary.option2;
            multipleChoiceThree.GetComponentInChildren<Text>().text = currentDictionary.option3;
            multipleChoiceFour.GetComponentInChildren<Text>().text = currentDictionary.option4;

        }
        else if (currentDictionary.type == "Set Variable")
        {
            DialogueManager.instance.SetBlackboardVariable(currentDictionary.blackboardString, currentDictionary.variableString, currentDictionary.variableValue);
            FindNextNode();

        }
        else if (currentDictionary.type == "Random")
        {
            List<string> inNodes = new List<string>();

            foreach (JSONConnectionDictionary currentRandom in currentJSONList.connectionList)
            {
                if (currentRandom.outPointID == currentDictionary.nodeID) inNodes.Add(currentRandom.inPointID);
            }

            string randomStr = inNodes[Random.Range(0, inNodes.Count)];
            foreach (JSONDictionary currentNode in currentJSONList.dataList)
            {
                if (currentNode.nodeID == randomStr)
                {
                    currentDictionary = currentNode;
                    ProcessNode();

                }
            }

        }
        else if (currentDictionary.type == "Boolean")
        {
            foreach (JSONConnectionDictionary currentRandom in currentJSONList.connectionList)
            {
                if (currentRandom.outBoolType == DialogueManager.instance.GetBlackboardVariable(currentDictionary.blackboardString, currentDictionary.variableString))
                {

                    if (currentRandom.outPointID == currentDictionary.nodeID)
                    {

                        foreach (JSONDictionary currentNode in currentJSONList.dataList)
                        {
                            if (currentNode.nodeID == currentRandom.inPointID)
                            {
                                currentDictionary = currentNode;
                                ProcessNode();
                                return;
                            }
                        }
                    }

                }
            }

        }
        else if (currentDictionary.type == "End Conversation") inConversation = false;
    }

    IEnumerator TypeText(string characterName, string bodyString)
    {

        typeStatus = TypeStatus.Typing;

        speakerTextBox.text = characterName;
        bodyTextBox.text = "";

        bodyString = bodyString.Replace("…", "...");

        foreach (char letter in bodyString.ToCharArray())
        {

            if (typeStatus == TypeStatus.Complete)
            {
                bodyTextBox.text = bodyString;

            }
            else
            {

                bodyTextBox.text += letter;
                if (letter == ".".ToCharArray()[0]) yield return new WaitForSeconds(.5f);
                else if (letter == ",".ToCharArray()[0]) yield return new WaitForSeconds(.2f);
                else if (letter == "!".ToCharArray()[0]) yield return new WaitForSeconds(.6f);
                else if (letter == "?".ToCharArray()[0]) yield return new WaitForSeconds(.6f);
                else yield return new WaitForSeconds(.03f);

            }
        }

        typeStatus = TypeStatus.Complete;

    }


    void PressOne()
    {
        selectedOption = 1;
        HideButtons();
        ProcessOption();
    }

    void PressTwo()
    {
        selectedOption = 2;
        HideButtons();
        ProcessOption();
    }

    void PressThree()
    {
        selectedOption = 3;
        HideButtons();
        ProcessOption();
    }

    void PressFour()
    {
        selectedOption = 4;
        HideButtons();
        ProcessOption();
    }

    void HideButtons() {
        multipleChoiceOne.gameObject.SetActive(false);
        multipleChoiceTwo.gameObject.SetActive(false);
        multipleChoiceThree.gameObject.SetActive(false);
        multipleChoiceFour.gameObject.SetActive(false);
    }

    void ProcessOption()
    {
        isMultipleChoice = false;
        foreach (JSONConnectionDictionary currentRandom in currentJSONList.connectionList)
        {
            if (currentRandom.optionNumber == selectedOption)
            {

                if (currentRandom.outPointID == currentDictionary.nodeID)
                {

                    foreach (JSONDictionary currentNode in currentJSONList.dataList)
                    {
                        if (currentNode.nodeID == currentRandom.inPointID)
                        {
                            currentDictionary = currentNode;
                            ProcessNode();
                            return;
                        }
                    }
                }

            }
        }

        
    }

    #endregion
}
