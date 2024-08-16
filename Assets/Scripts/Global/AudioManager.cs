using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource chaseAudio;
    [SerializeField] private AudioClip chaseClip;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
    }
    public void PlayerChasingAudio()
    {
        chaseAudio.PlayOneShot(chaseClip);
    }
    public void StopChasingAudio()
    {
        chaseAudio.Stop();
    }
}
