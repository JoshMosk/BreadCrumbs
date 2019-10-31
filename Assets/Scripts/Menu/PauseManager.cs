using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {

    // UI Panels
    public GameObject pausePanel;
    public GameObject creditsPanel;

    // Main Buttons
    public Button resumeButton;
    public Button restartButton;
    public Button creditsButton;
    public Button exitButton;

    // Back Buttons
    public Button creditsBackButton;

    // View State
    CurrentView currentView;
    public enum CurrentView {
        GameplayView,
        PauseView,
        CreditsView
    }

    void Start() {
        // Set the view to the main menu
        SetView(CurrentView.GameplayView);

        // Assign buttons
        resumeButton.GetComponent<Button>().onClick.AddListener(PressResume);
        restartButton.GetComponent<Button>().onClick.AddListener(PressRestart);
        creditsButton.GetComponent<Button>().onClick.AddListener(PressCredits);
        exitButton.GetComponent<Button>().onClick.AddListener(PressExit);
        creditsBackButton.GetComponent<Button>().onClick.AddListener(PressBack);

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) PressPause();
    }

    void SetView(CurrentView requestedView) {
        currentView = requestedView;

        // Show and hide panels depending on view
        switch (currentView) {
            case CurrentView.GameplayView:
                Time.timeScale = 1.0f;
                pausePanel.SetActive(false);
                creditsPanel.SetActive(false);
                break;

            case CurrentView.PauseView:
                Time.timeScale = 0.0f;
                pausePanel.SetActive(true);
                creditsPanel.SetActive(false);
                break;

            case CurrentView.CreditsView:
                pausePanel.SetActive(false);
                creditsPanel.SetActive(true);
                break;
        }
    }

    void PressPause() {
        SetView(CurrentView.PauseView);
    }

    void PressResume() {
        SetView(CurrentView.GameplayView);
    }

    void PressRestart() {
        
    }

    void PressCredits() {
        SetView(CurrentView.CreditsView);
    }

    void PressExit() {
        Application.Quit();
    }

    void PressBack() {
        SetView(CurrentView.GameplayView);
    }
}
