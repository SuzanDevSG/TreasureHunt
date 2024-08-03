using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;


public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button exitButton;
    private bool isPaused = false;
    

    void Start()
    {
        pauseMenuUI.SetActive(false);

        resumeButton.onClick.AddListener(Resume);
        exitButton.onClick.AddListener(ExitToMainMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
        isPaused = true;
    }

    void ExitToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("ui");
    }
}


