using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteSPPlayer : MonoBehaviour
{
    private bool hasDeletedPlayer = false;  // Track if the player has been deleted in this scene

    // Called when the script instance is being loaded
    void Start()
    {
        DeletePlayer(); // Try to delete the player when the scene starts

        // Subscribe to the sceneLoaded event to run the deletion again when a new scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Called when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        hasDeletedPlayer = false; // Reset the flag for the new scene
        DeletePlayer(); // Try to delete the player again in the new scene
    }

    // Function to delete the player object if it exists
    void DeletePlayer()
    {
        if (!hasDeletedPlayer)
        {
            // Look for the GameObject that has the exact name "Player"
            GameObject player = GameObject.Find("/Player"); // Absolute path search to find the object in the hierarchy

            if (player != null)
            {
                Destroy(player); // Destroy the player object
                Debug.Log("Player object deleted");
            }
            else
            {
                Debug.Log("Player object not found");
            }

            hasDeletedPlayer = true; // Prevents multiple deletions in the same scene
        }
    }

    // Unsubscribe from the sceneLoaded event when the script is destroyed
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
