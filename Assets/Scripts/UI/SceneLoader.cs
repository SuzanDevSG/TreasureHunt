using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadLevel1()
    {
        SceneManager.LoadScene(2); 
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene(3);
    }
}
