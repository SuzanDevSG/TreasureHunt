using UnityEngine;
using System.Collections.Generic;

public class PlayerKey : MonoBehaviour
{
    // Stores acquired keys in a HashSet
    public HashSet<int> acquiredKeys = new HashSet<int>();
    [SerializeField] private GameObject levelComplete;
    [SerializeField] private ItemScript MainChest;
    [SerializeField] private GameObject Interact;

    // Method to acquire a key
    public void AcquireKey(int keyId)
    {
        Interact.SetActive(false);
        acquiredKeys.Add(keyId);
    }

    // Method to check if player has a specific key
    public bool HasKey(int keyId)
    {
        return acquiredKeys.Contains(keyId);
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("MainChest")){
            Debug.Log("Chest Collided");
            if (!MainChest.isOpen) return;
            Debug.Log("Chest Opened");
            Invoke(nameof(OpenLevelCompletePanel), 1f);
        }

        if (other.CompareTag("Interactable"))
        {
            Interact.SetActive(true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            Interact.SetActive(false);
        }
        
    }
    private void OpenLevelCompletePanel()
    {
        levelComplete.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("LevelCompleted");

    }
}
