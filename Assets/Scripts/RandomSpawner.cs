using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    // Array to hold possible spawn points
    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        // Call the Spawn method to spawn the object at a random position
        Spawn();
    }

    // Method to spawn the object at a random position
    void Spawn()
    {
        // Check if there are spawn points assigned
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        // Get a random index from the spawnPoints array
        int randomIndex = Random.Range(0, spawnPoints.Length);

        // Set the object's position to the chosen spawn point's position
        transform.position = spawnPoints[randomIndex].position;

        // Optionally, set the object's rotation to match the spawn point's rotation
        transform.rotation = spawnPoints[randomIndex].rotation;
    }
}
