using UnityEngine;

public class CursorInteract : MonoBehaviour
{
    public string[] usableTags = { "Usable", "Door" }; // Add or remove tags as needed
    public float interactionDistance = 3f; // Adjust this distance based on your preference
    public GameObject crosshair;

    private GameObject currentUsableObject;

    private void Update()
    {
        CheckForObjectInteraction();
    }

    private void CheckForObjectInteraction()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Check if the hit object has one of the specified usable tags
            if (ArrayContainsTag(hitObject.tag))
            {
                // Enable crosshair and set the current usable object
                EnableCrosshair();
                currentUsableObject = hitObject;
            }
            else
            {
                // Disable crosshair if not looking at a usable object
                DisableCrosshair();
                currentUsableObject = null;
            }
        }
        else
        {
            // Disable crosshair if not looking at anything
            DisableCrosshair();
            currentUsableObject = null;
        }
    }

    private bool ArrayContainsTag(string tag)
    {
        foreach (string usableTag in usableTags)
        {
            if (tag.Equals(usableTag))
            {
                return true;
            }
        }
        return false;
    }

    private void EnableCrosshair()
    {
        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }
    }

    private void DisableCrosshair()
    {
        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }
    }
}
