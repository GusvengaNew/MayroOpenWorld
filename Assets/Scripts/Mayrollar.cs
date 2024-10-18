using UnityEngine;
using UnityEngine.UI; // Import the UI namespace
using Random = UnityEngine.Random;

public class Mayrollar : MonoBehaviour
{
    public string moneyTag = "money"; // Customize the tag for your money objects
    public int minMoneyAmount = 5;   // Minimum amount of money to add
    public int maxMoneyAmount = 20;  // Maximum amount of money to add

    public AudioSource audioSource;  // Reference to the AudioSource component
    public AudioClip[] collectedSounds; // Array of collected sounds

    public Text moneyCountText; // Reference to the UI Text component
    public float moveSpeed = 5f; // Speed at which money moves towards the player

    private int moneyCount = 0;

    private void Update()
    {
        MoveMoneyTowardsPlayer();
    }

    private void MoveMoneyTowardsPlayer()
    {
        GameObject[] moneyObjects = GameObject.FindGameObjectsWithTag(moneyTag);

        foreach (GameObject moneyObject in moneyObjects)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, moneyObject.transform.position);

            // Check if the player is close to the money object
            if (distanceToPlayer < 5f)
            {
                // Move the money object towards the player using lerp
                moneyObject.transform.position = Vector3.Lerp(
                    moneyObject.transform.position,
                    transform.position,
                    moveSpeed * Time.deltaTime
                );
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(moneyTag))
        {
            int randomMoneyAmount = Random.Range(minMoneyAmount, maxMoneyAmount + 1);
            moneyCount += randomMoneyAmount;

            // Play a random selected sound
            if (collectedSounds.Length > 0 && audioSource != null)
            {
                int randomSoundIndex = Random.Range(0, collectedSounds.Length);
                audioSource.PlayOneShot(collectedSounds[randomSoundIndex]);
            }

            // Remove the collected money object
            Destroy(other.gameObject);

            // Update the UI Text with the current money count
            if (moneyCountText != null)
            {
                moneyCountText.text = $"Moneh: {moneyCount}";
            }

            Debug.Log($"Money collected! Amount: {randomMoneyAmount}, Total money: {moneyCount}");
        }
    }
}
