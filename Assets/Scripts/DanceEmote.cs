using UnityEngine;

public class DanceEmote : MonoBehaviour
{
    // Reference to the GameObject to be activated
    public GameObject objectToActivate;
    // Reference to the GameObject to be deactivated
    public GameObject objectToDeactivate;

    void Update()
    {
        // Check if the Tab key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Activate the first GameObject
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
            
            // Deactivate the second GameObject
            if (objectToDeactivate != null)
            {
                objectToDeactivate.SetActive(false);
            }
        }
    }
}
