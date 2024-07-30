using UnityEngine;

public class ItemKeyInitializer : MonoBehaviour
{
    public GameObject keyPrefab;
    public int itemId;

    void Start()
    {
        // Initialize the item ID
        itemId = UniqueIDGenerator.GetNextID(); // Assign unique ID to item
        if (keyPrefab != null)
        {
            KeyScript keyScript = keyPrefab.GetComponent<KeyScript>();
            if (keyScript != null)
            {
                keyScript.keyId = itemId; // Assign the same ID to the key
            }
        }
    }
}