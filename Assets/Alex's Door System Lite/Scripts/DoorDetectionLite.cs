// DoorDetectionLite.cs
// Created by Alexander Ameye
// Version 3.0.0

using UnityEngine;

[HelpURL("https://alexdoorsystem.github.io/liteversion/")]
public class DoorDetectionLite : MonoBehaviour
{
    // UI Settings
    public GameObject TextPrefab; // A text element to display when the player is in reach of the door
    [HideInInspector] public GameObject TextPrefabInstance; // A copy of the text prefab to prevent data corruption
    [HideInInspector] public bool TextActive;

    public GameObject CrosshairPrefab;
    [HideInInspector] public GameObject CrosshairPrefabInstance; // A copy of the crosshair prefab to prevent data corruption

    // Raycast Settings
    public float Reach = 4.0F;
    [HideInInspector] public bool InReach;
    public string Character = "e";

    public Color DebugRayColor;

    private Transform playerCamera;
    private Collider playerCollider;

    void Start()
    {
        gameObject.name = "Player";
        gameObject.tag = "Player";

        playerCamera = Camera.main.transform;
        playerCollider = GetComponent<Collider>();

        if (CrosshairPrefab == null) Debug.Log("<color=yellow><b>No CrosshairPrefab was found.</b></color>"); // Return an error if no crosshair was specified
        else
        {
            CrosshairPrefabInstance = Instantiate(CrosshairPrefab); // Display the crosshair prefab
            CrosshairPrefabInstance.transform.SetParent(transform, true); // Make the player the parent object of the crosshair prefab
        }

        if (TextPrefab == null) Debug.Log("<color=yellow><b>No TextPrefab was found.</b></color>"); // Return an error if no text element was specified
    }

    void Update()
    {
        // Set origin of ray to 'camera's position' and direction of ray to 'forward direction of the camera'
        Vector3 rayOrigin = playerCamera.position;
        Vector3 rayDirection = playerCamera.forward;

        Ray ray = new Ray(rayOrigin, rayDirection);
        RaycastHit[] hits = Physics.RaycastAll(ray, Reach);

        RaycastHit? doorHit = null;

        // Filter out the player's collider from the hit results
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != playerCollider)
            {
                if (hit.collider.CompareTag("Door"))
                {
                    doorHit = hit;
                    break;
                }
            }
        }

        if (doorHit.HasValue)
        {
            InReach = true;
            RaycastHit hit = doorHit.Value;

            // Display the UI element when the player is in reach of the door
            if (TextActive == false && TextPrefab != null)
            {
                TextPrefabInstance = Instantiate(TextPrefab);
                TextActive = true;
                TextPrefabInstance.transform.SetParent(transform, true); // Make the player the parent object of the text element
            }

            // Give the object that was hit the name 'Door'
            GameObject Door = hit.transform.gameObject;

            // Get access to the 'Door' script attached to the object that was hit
            DoorRotationLite dooropening = Door.GetComponent<DoorRotationLite>();

            if (Input.GetKey(Character))
            {
                // Open/close the door by running the 'Open' function found in the 'Door' script
                if (dooropening.RotationPending == false) StartCoroutine(hit.collider.GetComponent<DoorRotationLite>().Move());
            }
        }
        else
        {
            InReach = false;

            // Destroy the UI element when Player is no longer in reach of the door
            if (TextActive == true)
            {
                DestroyImmediate(TextPrefabInstance);
                TextActive = false;
            }
        }

        // Draw the ray as a colored line for debugging purposes.
        Debug.DrawRay(ray.origin, ray.direction * Reach, DebugRayColor);
    }
}
