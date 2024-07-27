using UnityEngine;

public class ChestController : MonoBehaviour
{
    public Animator chestAnimator;  // Reference to the Animator component

    void Start()
    {
        if (chestAnimator == null)
        {
            chestAnimator = GetComponent<Animator>();
            // Get the Animator component attached to the chest
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chestAnimator.SetBool("Open", true);  // Sets the 'Open' parameter to true
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chestAnimator.SetBool("Open", false);  // Sets the 'Open' parameter to false
        }
    }
}
