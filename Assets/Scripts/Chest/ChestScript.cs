using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public Animator animator;
    private int chestId; // Unique ID for this chest
    private bool isPlayerNearby = false;
    private bool isOpen = false;
    private GameObject player;

    void Start()
    {
        // Make sure animator is assigned
        animator = GetComponent<Animator>();

        // Check if the script is attached to the chest
        ChestKeyInitializer initializer = GetComponentInParent<ChestKeyInitializer>();
        if (initializer != null)
        {
            chestId = initializer.chestId; // Get the ID from the initializer
        }
    }

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
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            PlayerKey playerKey = player.GetComponent<PlayerKey>();
            if (playerKey != null && playerKey.HasKey(chestId))
            {
                OpenChest();
            }
        }
    }

    void OpenChest()
    {
        isOpen = true;
        animator.SetBool("isOpen", true);
    }

    void CloseChest()
    {
        isOpen = false;
        animator.SetBool("isOpen", false);
    }
}