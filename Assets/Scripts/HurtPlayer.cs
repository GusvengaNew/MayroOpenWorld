using UnityEngine;
using System.Collections;

public class HurtPlayer : MonoBehaviour
{
    public int damageAmount = 25;
    private bool canDamage = true;
    private float damageCooldown = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                StartCoroutine(DamagePlayer(playerHealth));
            }
        }
    }

    private IEnumerator DamagePlayer(PlayerHealth playerHealth)
    {
        canDamage = false;
        playerHealth.TakeDamage(damageAmount);
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
