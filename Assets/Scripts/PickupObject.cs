using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public Transform handlingPosition; // Reference to the handling position
    public float grabRange = 2f; // Range within which we can grab objects
    public float throwForce = 10f; // Force applied to the object when thrown
    private GameObject currentObject; // The currently grabbed object
    private Rigidbody currentObjectRb; // The Rigidbody of the currently grabbed object

    void Update()
    {
        // Check for E key press
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentObject == null)
            {
                TryGrabObject();
            }
            else
            {
                DropObject();
            }
        }

        // Check for left mouse button press to throw the object
        if (Input.GetMouseButtonDown(0) && currentObject != null)
        {
            ThrowObject();
        }

        // If an object is grabbed, sync its position with the handling position
        if (currentObject != null)
        {
            SyncObjectPosition();
        }
    }

    void TryGrabObject()
    {
        // Find all colliders within grab range
        Collider[] colliders = Physics.OverlapSphere(transform.position, grabRange);
        foreach (Collider collider in colliders)
        {
            // Check if the object has the "Object" tag or the "Key" tag
            if (collider.CompareTag("Object") || collider.CompareTag("Key"))
            {
                // Grab the object
                currentObject = collider.gameObject;
                currentObjectRb = currentObject.GetComponent<Rigidbody>();
                if (currentObjectRb != null)
                {
                    currentObjectRb.useGravity = false; // Optionally disable gravity while grabbing
                }
                break;
            }
        }
    }

    void DropObject()
    {
        if (currentObject != null)
        {
            // Detach the object
            if (currentObjectRb != null)
            {
                currentObjectRb.useGravity = true; // Re-enable gravity if it was disabled
                currentObjectRb = null;
            }
            currentObject = null;
        }
    }

    void ThrowObject()
    {
        if (currentObject != null)
        {
            // Apply force to throw the object
            if (currentObjectRb != null)
            {
                currentObjectRb.useGravity = true; // Re-enable gravity if it was disabled
                currentObjectRb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
                currentObjectRb = null;
            }
            currentObject = null;
        }
    }

    void SyncObjectPosition()
    {
        if (currentObject != null)
        {
            currentObject.transform.position = handlingPosition.position;
            currentObject.transform.rotation = handlingPosition.rotation;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the grab range in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grabRange);
    }
}
