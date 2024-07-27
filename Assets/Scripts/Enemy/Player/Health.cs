using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle player death (e.g., disable player controls, show game over screen, etc.)
        if (currentHealth < 0)
        {
            gameObject.SetActive(false);        }
        Debug.Log("Player died");
    }
}
