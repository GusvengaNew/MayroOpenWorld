using UnityEngine;

public class MoonMap : MonoBehaviour
{
    // Reference to the object you want to enable
    public GameObject objectToEnable;

    // Reference to the object you want to disable
    public GameObject objectToDisable;

    // Ensure the objects are assigned in the Inspector
    private void Start()
    {
        if (objectToEnable == null || objectToDisable == null)
        {
            Debug.LogError("Please assign both objects in the Inspector.");
            enabled = false; // Disable this script if the setup is incomplete
        }
    }

    // This method is called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Enable the first object
            objectToEnable.SetActive(true);

            // Disable the second object
            objectToDisable.SetActive(false);
        }
    }

    // This method is called when another collider exits the trigger collider attached to this object
    private void OnTriggerExit(Collider other)
    {
        // Check if the object that exited the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Disable the first object
            objectToEnable.SetActive(false);

            // Enable the second object
            objectToDisable.SetActive(true);
        }
    }
}
