using UnityEngine;
using System.Collections.Generic;

public class PlayerKey : MonoBehaviour
{
    // Stores acquired keys in a HashSet
    public HashSet<int> acquiredKeys = new HashSet<int>();

    // Method to acquire a key
    public void AcquireKey(int keyId)
    {
        acquiredKeys.Add(keyId);
    }

    // Method to check if player has a specific key
    public bool HasKey(int keyId)
    {
        return acquiredKeys.Contains(keyId);
    }
}
