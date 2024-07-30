using UnityEngine;

public class ChestKeyInitializer : MonoBehaviour
{
    public GameObject keyPrefab;
    public int chestId;

    void Start()
    {
        // Initialize the chest ID
        chestId = UniqueIDGenerator.GetNextID(); // Assign unique ID to chest
        if (keyPrefab != null)
        {
            KeyScript keyScript = keyPrefab.GetComponent<KeyScript>();
            if (keyScript != null)
            {
                keyScript.keyId = chestId; // Assign the same ID to the key
            }
        }
    }
}