using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;


public class SceneController : MonoBehaviour
{
    public static SceneController Instance { set; get; }
    
    public List<SceneDep> sceneDeps;

    public SceneAsset startScene;


    //public void Load(string sceneName)
    //{
    //    if(!SceneManager.GetSceneByName(sceneName).isLoaded)
    //    {
    //        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    //    }
    //}

    //public void Unload(string sceneName)
    //{
    //    if(SceneManager.GetSceneByName(sceneName).isLoaded)
    //    {
    //        SceneManager.UnloadSceneAsync(sceneName);
    //    }
    //}

    private void Start()
    {
        //SceneManager.LoadSceneAsync(SceneManager.GetSceneByName("test1").buildIndex, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(startScene.name, LoadSceneMode.Additive);
    }

    public void LoadDepFromTargetScene(SceneAsset s)
    {
        SceneDep temp = sceneDeps.Find((SceneDep a) => { return a.currentScene == s; });

        foreach (Scene x in SceneManager.GetAllScenes())//for every scene currely loaded
        {
            //if scene loaded isnt current or dep get it out //currently the thing is yes when it meant be no
            if (x.name != temp.currentScene.name )
            {
                if( temp.dependantScenes.Find((SceneAsset a) => { return a.name == x.name; }) == null)
                SceneManager.UnloadSceneAsync(x.name);
            }
        }
        //go through scenedep and make sure all scenes are loaded
        foreach(SceneAsset x in temp.dependantScenes)
        {
            Scene y = SceneManager.GetSceneByName(x.name);
            if(SceneManager.GetSceneByName(x.name) == null)
            {
                SceneManager.LoadSceneAsync(x.name, LoadSceneMode.Additive);
            }
        }

    }
    
}

[System.Serializable]
public class SceneDep
{
    //have the scenes that this scene is dependant on
    public SceneAsset currentScene;
    public List<SceneAsset> dependantScenes;

}

