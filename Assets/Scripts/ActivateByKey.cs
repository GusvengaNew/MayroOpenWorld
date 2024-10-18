using UnityEngine;

public class ActivateByKey : MonoBehaviour
{
    // Key to press to activate the game objects
    public KeyCode activationKey = KeyCode.Space;

    // Key to press to deactivate the game object this script is attached to
    public KeyCode deactivationKey = KeyCode.Tab;

    // Array to hold the game objects to be activated
    public GameObject[] gameObjectsToActivate;

    void Update()
    {
        // Check if the specified key is pressed to activate objects
        if (Input.GetKeyDown(activationKey))
        {
            ActivateObjects();
        }

        // Check if the specified key is pressed to deactivate this object
        if (Input.GetKeyDown(deactivationKey))
        {
            // Deactivate this object after a slight delay to ensure activation happens first
            StartCoroutine(DeactivateAfterActivation());
        }
    }

    void ActivateObjects()
    {
        // Iterate through the array and set each game object to active
        foreach (GameObject obj in gameObjectsToActivate)
        {
            obj.SetActive(true);
        }
    }

    System.Collections.IEnumerator DeactivateAfterActivation()
    {
        // Ensure all objects are activated first
        ActivateObjects();

        // Wait for a frame to ensure activation happens
        yield return null;

        // Deactivate this game object
        gameObject.SetActive(false);
    }
}
