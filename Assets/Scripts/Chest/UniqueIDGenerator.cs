using UnityEngine;

public class UniqueIDGenerator : MonoBehaviour
{
    private static UniqueIDGenerator instance;
    private static int nextID = 0;

    // Singleton pattern to ensure only one instance
    public static UniqueIDGenerator Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UniqueIDGenerator>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("UniqueIDGenerator");
                    instance = obj.AddComponent<UniqueIDGenerator>();
                }
            }
            return instance;
        }
    }

    public static int GetNextID()
    {
        return Instance.GetNextIDInternal();
    }

    private int GetNextIDInternal()
    {
        return nextID++;
    }
}


