using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;


public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject HUD;
    public Button resumeButton;
    public Button exitButton;
    private bool IsPaused = false;
    public Slider volumeSlider;
    [SerializeField] private PlayerController playerController;
    void Start()
    {
        pauseMenuUI.SetActive(false);

        resumeButton.onClick.AddListener(Resume);
        exitButton.onClick.AddListener(ExitToMainMenu);
        volumeSlider.onValueChanged.AddListener(AdjustVolume);
    }

    void Update()
    {

        if (playerController.PauseAction.triggered && !PetrollingEnemy.IsGameOver)
        {
            Pause();
        }
        if (playerController.PauseAction.triggered && !PetrollingEnemy.IsGameOver && !IsPaused)
        {
            Resume();
        }
    }

    public void Resume()
    {
        HUD.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //if (playerController.IsbeingChased)
        //{
        //    playerController.StartPlayingAudio();
        //}
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    void Pause()
    {
        HUD.SetActive(false);
        Cursor.lockState = CursorLockMode.None;

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
        IsPaused = !IsPaused;
       
    }

    void ExitToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");
    }
    void AdjustVolume(float volume)
    {
        AudioListener.volume = volume;

    }
}


