using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInfo : MonoBehaviour {
    public static Dictionary<string, object> GetInfo(Node.NodeType currentType, string uniqueIDString) {

        // Setup each kind of node and its details
        List<Dictionary<string, object>> nodeFields = new List<Dictionary<string, object>>();
        Dictionary<string, object> nodeInfo = new Dictionary<string, object>();
        switch (currentType) {
            case Node.NodeType.ConversationNode:

                nodeInfo.Add("name", "Conversation");
                nodeInfo.Add("description", "Begins a new thread of dialogue nodes");
                nodeInfo.Add("height", 138f);
                nodeInfo.Add("width", 180f);
                nodeInfo.Add("inConnectionNumber", 0);
                nodeInfo.Add("outConnectionNumber", 1);

                nodeFields.Add(new Dictionary<string, object> { { "name", "Name" }, { "kind", "SmallBox" }, { "id", "name" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "ID: " + uniqueIDString }, { "kind", "Copy" }, { "id", "uniqueID" } });

                break;


            case Node.NodeType.DialogueNode:

                nodeInfo.Add("name", "Dialogue");
                nodeInfo.Add("description", "Defines a speach bubble in a conversation");
                nodeInfo.Add("height", 200f);
                nodeInfo.Add("width", 200f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 1);

                nodeFields.Add(new Dictionary<string, object> { { "name", "Speaker" }, { "kind", "Dropdown" }, { "id", "speaker" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Emotion" }, { "kind", "FullDropdown" }, { "id", "emotion" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Dialogue" }, { "kind", "LargeBox" }, { "id", "dialogue" } });

                break;


            case Node.NodeType.CommentNode:

                nodeInfo.Add("name", "Comment");
                nodeInfo.Add("description", "Can be used to describe a conversation");
                nodeInfo.Add("height", 150f);
                nodeInfo.Add("width", 200f);
                nodeInfo.Add("inConnectionNumber", 0);
                nodeInfo.Add("outConnectionNumber", 0);

                nodeFields.Add(new Dictionary<string, object> { { "name", "" }, { "kind", "LargeBox" }, { "id", "comment" } });

                break;


            case Node.NodeType.EndNode:

                nodeInfo.Add("name", "End Conversation");
                nodeInfo.Add("description", "Concludes the preceding conversation");
                nodeInfo.Add("height", 57f);
                nodeInfo.Add("width", 160f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 0);

                break;


            case Node.NodeType.GetBooleanNode:

                nodeInfo.Add("name", "Get Boolean");
                nodeInfo.Add("description", "Returns true or false based on a blackboard value");
                nodeInfo.Add("height", 135f);
                nodeInfo.Add("width", 200f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 2);

                nodeFields.Add(new Dictionary<string, object> { { "name", "Blackboard" }, { "kind", "Dropdown" }, { "id", "blackboard" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Variable" }, { "kind", "Dropdown" }, { "id", "variable" } });

                break;


            case Node.NodeType.SetBooleanNode:

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


            case Node.NodeType.MultipleChoiceNode:

                nodeInfo.Add("name", "Multiple Choice");
                nodeInfo.Add("description", "Allows different options for the player to choose from");
                nodeInfo.Add("height", 350f);
                nodeInfo.Add("width", 200f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 4);

                nodeFields.Add(new Dictionary<string, object> { { "name", "Speaker" }, { "kind", "Dropdown" }, { "id", "speaker" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Emotion" }, { "kind", "FullDropdown" }, { "id", "emotion" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Dialogue" }, { "kind", "LargeBox" }, { "id", "dialogue" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Option 1" }, { "kind", "SmallBox" }, { "id", "option1" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Option 2" }, { "kind", "SmallBox" }, { "id", "option2" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Option 3" }, { "kind", "SmallBox" }, { "id", "option3" } });
                nodeFields.Add(new Dictionary<string, object> { { "name", "Option 4" }, { "kind", "SmallBox" }, { "id", "option4" } });

                break;


            case Node.NodeType.RandomNode:

                nodeInfo.Add("name", "Random");
                nodeInfo.Add("description", "Picks a random node");
                nodeInfo.Add("height", 57f);
                nodeInfo.Add("width", 150f);
                nodeInfo.Add("inConnectionNumber", 1);
                nodeInfo.Add("outConnectionNumber", 1);

                break;


            case Node.NodeType.BroadcastNode:

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

}
