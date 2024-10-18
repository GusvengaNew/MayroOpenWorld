using UnityEngine;
using System.Collections.Generic;

public class BooserObjectDistract : MonoBehaviour
{
    public BooserNPC npc;
    public float movementThreshold = 0.1f;  // Minimum movement to trigger the distraction
    public float delayTime = 3.0f;          // Delay before the distraction system starts functioning

    private Vector3 lastPosition;
    private bool isDelayOver = false;
    private float delayEndTime;
    private static HashSet<Transform> movedObjects = new HashSet<Transform>();

    void Start()
    {
        lastPosition = transform.position;
        delayEndTime = Time.time + delayTime;  // Set the time when the delay will end
    }

    void Update()
    {
        // Check if the delay period has ended
        if (Time.time >= delayEndTime)
        {
            isDelayOver = true;  // Enable the distraction system after the delay
        }

        // Check if the object has moved more than the threshold
        if (Vector3.Distance(lastPosition, transform.position) > movementThreshold)
        {
            lastPosition = transform.position;  // Update the last position after movement

            if (isDelayOver)
            {
                // Store the object if it has moved after the delay period
                movedObjects.Add(transform);
            }
        }

        // React to objects that have moved, if the delay is over and the object is in sight
        if (isDelayOver && movedObjects.Contains(transform))
        {
            if (npc.CanSeeObject(transform.position))
            {
                npc.ReactToSound(transform.position);
                movedObjects.Remove(transform);  // Reset the moved flag after reacting
            }
        }
    }
}
