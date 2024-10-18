using UnityEngine;

public class SaveText : MonoBehaviour
{
    public GameObject objectToActivate; // Reference to the object you want to activate
    public KeyCode activationKey = KeyCode.Space; // Key to activate the object
    public float activationDuration = 2.0f; // Duration in seconds for which the object will stay active

    private bool isActivated = false;
    private float activationStartTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        // Check if the activation key is pressed
        if (Input.GetKeyDown(activationKey))
        {
            // Check if the object to activate is not null
            if (objectToActivate != null)
            {
                // Activate the object if it's currently inactive
                if (!isActivated)
                {
                    objectToActivate.SetActive(true);
                    isActivated = true;
                    activationStartTime = Time.realtimeSinceStartup;
                    Debug.Log("Object activated!");
                }
                else
                {
                    Debug.Log("Object is already activated!");
                }
            }
            else
            {
                Debug.LogError("Object to activate is not assigned!");
            }
        }

        // If the object is activated, check if it's time to deactivate
        if (isActivated)
        {
            float elapsedTime = Time.realtimeSinceStartup - activationStartTime;
            // Deactivate the object when the elapsed time exceeds the activation duration
            if (elapsedTime >= activationDuration)
            {
                objectToActivate.SetActive(false);
                isActivated = false;
                Debug.Log("Object deactivated!");
            }
        }
    }
}
