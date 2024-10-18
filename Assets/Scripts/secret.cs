using UnityEngine;
using UnityEngine.SceneManagement;

public class secret : MonoBehaviour
{
    // Chance for the scene to change (e.g., 0.001f = 0.1% chance)
    [Range(0f, 1f)]
    public float chance = 0.001f;

    // Name of the scene to load
    public string sceneToLoad;

    void Start()
    {
        TryChangeScene();
    }

    void TryChangeScene()
    {
        // Generate a random number between 0 and 1
        float randomValue = Random.Range(0f, 1f);

        // If the random value is less than the chance, change the scene
        if (randomValue < chance)
        {
            // Load the specified scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
