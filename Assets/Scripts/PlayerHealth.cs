using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Text healthText;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // Handle game over or respawn logic here
            GameOver();
        }
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthText.text = "Healf: " + currentHealth.ToString();
    }

    private void GameOver()
    {
        // For demonstration, we reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
