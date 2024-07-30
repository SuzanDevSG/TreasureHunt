using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Animator animator;
    private int itemId; // Unique ID for this item
    private bool isPlayerNearby = false;
    private bool isOpen = false;
    private GameObject player;

    void Start()
    {
        // Make sure animator is assigned
        animator = GetComponent<Animator>();

        // Check if the script is attached to the item
        ItemKeyInitializer initializer = GetComponentInParent<ItemKeyInitializer>();
        if (initializer != null)
        {
            itemId = initializer.itemId; // Get the ID from the initializer
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
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PlayerKey playerKey = player.GetComponent<PlayerKey>();
            if (playerKey != null && playerKey.HasKey(itemId))
            {
                ToggleItem();
            }
        }
    }

    void ToggleItem()
    {
        if (isOpen)
        {
            CloseItem();
        }
        else
        {
            OpenItem();
        }
    }

    void OpenItem()
    {
        isOpen = true;
        animator.SetBool("isOpen", true);
    }

    void CloseItem()
    {
        isOpen = false;
        animator.SetBool("isOpen", false);
    }
}