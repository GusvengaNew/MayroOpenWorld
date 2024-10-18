using UnityEngine;

public class Deactivate : MonoBehaviour
{
    public KeyCode deactivateKey = KeyCode.Space; // Change this to the desired input key

    // Update is called once per frame
    void Update()
    {
        // Check if the specified key is pressed
        if (Input.GetKeyDown(deactivateKey))
        {
            // Deactivate the GameObject
            gameObject.SetActive(false);
        }
    }
}
