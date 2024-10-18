using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public int healAmount = 25; // Amount of health to regenerate

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                gameObject.SetActive(false); // Deactivate the health object after regeneration
            }
        }
    }
}
