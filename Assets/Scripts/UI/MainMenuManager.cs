using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    
    public Button yesButton;
    public Slider volumeSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        yesButton.onClick.AddListener(QuitGame);
        volumeSlider.onValueChanged.AddListener(AdjustVolume);
    }

    // Update is called once per frame
    void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #elif UNITY_STANDALONE
        Application.Quit();
    #endif
 
        Debug.Log("Quit Game");
    }
    void AdjustVolume(float volume)
    {
        AudioListener.volume = volume;
        
    }
}
