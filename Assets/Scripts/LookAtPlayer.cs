using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    // Public variable to assign the target object in the Unity Inspector
    public Transform target;

    void Update()
    {
        // Check if target is assigned
        if (target != null)
        {
            // Get the direction to the target on the XZ plane
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // Ignore the Y component

            // Check if direction is not zero to avoid NaN errors
            if (direction.sqrMagnitude > 0.0f)
            {
                // Calculate the rotation needed to look at the target
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                // Apply the rotation only on the y-axis
                transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            }
        }
    }
}
