using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public int keyId; // Unique ID for this key
    private bool isPlayerNearby = false;
    private GameObject player;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            player = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            player = null;
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            AcquireKey();
        }
    }

    private void AcquireKey()
    {
        if (player != null)
        {
            PlayerKey playerKey = player.GetComponent<PlayerKey>();
            if (playerKey != null)
            {
                playerKey.AcquireKey(keyId);
                Destroy(gameObject);
            }
        }
    }
}