using UnityEngine;
using System.Collections.Generic;

public class BreakObject : MonoBehaviour
{
    // Speed threshold for adding a Rigidbody
    public float speedThreshold = 10.0f;

    // List of tags that can trigger the Rigidbody addition
    public List<string> allowedTags = new List<string>();

    // Called when the object collides with another object
    private void OnCollisionEnter(Collision collision)
    {
        // Calculate the relative speed of the collision
        float relativeSpeed = collision.relativeVelocity.magnitude;

        // Check if the speed exceeds the threshold
        if (relativeSpeed > speedThreshold)
        {
            // If allowedTags is not empty, check if the collision object's tag is in the list
            if (allowedTags.Count > 0)
            {
                if (!allowedTags.Contains(collision.gameObject.tag))
                {
                    return; // Exit if the tag is not in the allowed list
                }
            }

            // Check if the object already has a Rigidbody
            if (GetComponent<Rigidbody>() == null)
            {
                // Add a Rigidbody component
                gameObject.AddComponent<Rigidbody>();
                Debug.Log("Rigidbody added due to high-speed collision with an allowed object.");
            }
        }
    }
}
