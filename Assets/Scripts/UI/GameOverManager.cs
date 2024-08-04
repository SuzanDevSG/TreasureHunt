using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI; // Reference to the Game Over UI panel


    // Show the Game Over UI
    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);
        Debug.Log("GameOver Show");
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Restart the current level
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void LoadMainMenu()
    {
        // Replace "MainMenu" with the name of your main menu scene
        SceneManager.LoadScene("MainMenu");
    }
     public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
