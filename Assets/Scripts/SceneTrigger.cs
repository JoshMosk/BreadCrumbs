using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneTrigger : MonoBehaviour
{
    public SceneAsset currentScene;
    public SceneController control;

    public void Start()
    {
        control = GameObject.Find("SceneController").GetComponent<SceneController>();
    }

    private void OnTriggerEnter(Collider other)
    {
            Debug.Log(currentScene.name + " has loaded");
        if(other.tag == "Player")
        {
            //SceneController.Instance.LoadDepFromTargetScene(currentScene);
            control.LoadDepFromTargetScene(currentScene);
        }
    }
}
