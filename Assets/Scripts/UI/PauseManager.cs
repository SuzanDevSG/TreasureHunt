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
    public Slider volumeSlider;
    [SerializeField] private PetrollingEnemy petrollingEnemy;
    void Start()
    {
        pauseMenuUI.SetActive(false);

        resumeButton.onClick.AddListener(Resume);
        exitButton.onClick.AddListener(ExitToMainMenu);
        volumeSlider.onValueChanged.AddListener(AdjustVolume);
    }

    void Update()
    {
        if ( petrollingEnemy.pauseMenu && !petrollingEnemy.isGameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Pause();

        }
        if(!petrollingEnemy.pauseMenu & !petrollingEnemy.isGameOver)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Resume();
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
        petrollingEnemy.chaseAudioSource.Stop();
       
    }

    void ExitToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("ui");
    }
    void AdjustVolume(float volume)
    {
        AudioListener.volume = volume;

    }
}


