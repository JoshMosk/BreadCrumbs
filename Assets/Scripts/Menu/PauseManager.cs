using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {

    public GameObject pausePanel;

    public Button resumeButton;
    public Button menuButton;

    bool isPaused;

    void Start() {
        resumeButton.GetComponent<Button>().onClick.AddListener(PressResume);
        menuButton.GetComponent<Button>().onClick.AddListener(PressMenu);
        pausePanel.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = !isPaused;
            if (isPaused) Pause();
            else Unpause();
        }
    }

    void PressResume() {
        Unpause();
        pausePanel.SetActive(false);
    }

    void PressMenu() {
        Unpause();
        SceneManager.LoadScene(0);
    }

    void Pause() {
        isPaused = true;
        Time.timeScale = 0.0f;
        pausePanel.SetActive(true);
    }

    void Unpause() {
        isPaused = false;
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
    }
}
