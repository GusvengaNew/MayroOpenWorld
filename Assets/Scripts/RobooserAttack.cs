using System.Collections;
using UnityEngine;

public class RobooserAttack : MonoBehaviour
{
    // Prefab to be spawned
    public GameObject prefabToSpawn;
    
    // Total number of prefabs to spawn each cycle
    public int totalPrefabs = 10;

    // Time interval between spawns
    public float spawnInterval = 0.5f;

    // Time interval between cycles
    public float cycleInterval = 10f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCycle());
    }

    // Coroutine to handle the spawning cycle
    IEnumerator SpawnCycle()
    {
        while (true)
        {
            // Spawn the prefabs
            for (int i = 0; i < totalPrefabs; i++)
            {
                SpawnPrefab();
                yield return new WaitForSeconds(spawnInterval);
            }
            
            // Wait for the cycle interval before starting again
            yield return new WaitForSeconds(cycleInterval);
        }
    }

    // Method to spawn the prefab
    void SpawnPrefab()
    {
        Instantiate(prefabToSpawn, transform.position, transform.rotation);
    }
}
