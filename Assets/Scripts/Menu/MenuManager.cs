using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    // Main Buttons
    public Button playButton;
    public Button levelButton;
    public Button exitButton;

    // UI Panels
    public GameObject menuPanel;
    public GameObject levelPanel;

    // Back Buttons
    public Button levelBackButton;

    // View State
    CurrentView currentView;
    public enum CurrentView {

        MenuView,
        LevelView,

    }

    void Start () {

        // Assign buttons to methods
        playButton.GetComponent<Button>().onClick.AddListener(PressPlay);
        levelButton.GetComponent<Button>().onClick.AddListener(PressLevel);
        exitButton.GetComponent<Button>().onClick.AddListener(PressExit);
        levelBackButton.GetComponent<Button>().onClick.AddListener(PressBack);

        // Set the view to the main menu
        currentView = CurrentView.MenuView;
    }

    void Update() {

        // Show and hide panels depending on view
        switch (currentView) {
            case CurrentView.MenuView:
                menuPanel.SetActive(true);
                levelPanel.SetActive(false);
                break;

            case CurrentView.LevelView:
                menuPanel.SetActive(false);
                levelPanel.SetActive(true);
                break;
        }

    }

    void PressPlay() {
        SceneManager.LoadScene(1);
    }

    void PressLevel() {
        currentView = CurrentView.LevelView;
    }

    void PressExit() {
        Application.Quit();
    }

    void PressBack() {
        currentView = CurrentView.MenuView;
    }
}
