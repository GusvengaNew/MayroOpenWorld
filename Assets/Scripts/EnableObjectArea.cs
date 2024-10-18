using UnityEngine;

public class EnableObjectArea : MonoBehaviour
{
    // Reference to the object you want to enable/disable
    public GameObject objectToEnable;

    // Ensure the object is assigned in the Inspector
    private void Start()
    {
        if (objectToEnable == null)
        {
            Debug.LogError("Please assign the object to enable/disable in the Inspector.");
            enabled = false; // Disable this script if the setup is incomplete
        }
    }

    // This method is called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Enable the object
            objectToEnable.SetActive(true);
        }
    }

    // This method is called when another collider exits the trigger collider attached to this object
    private void OnTriggerExit(Collider other)
    {
        // Check if the object that exited the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Disable the object
            objectToEnable.SetActive(false);
        }
    }
}
