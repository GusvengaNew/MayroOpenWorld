using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Optimize : MonoBehaviour
{
    private Renderer objectRenderer;
    private Camera[] cameras;

    void Start()
    {
        // Get the Renderer component attached to this GameObject
        objectRenderer = GetComponent<Renderer>();

        // Retrieve all cameras in the scene
        cameras = Camera.allCameras;

        // Initial visibility check
        UpdateVisibility();
    }

    void Update()
    {
        // Update visibility status every frame
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (objectRenderer == null) return;

        bool isVisible = false;

        // Check visibility from all cameras
        foreach (Camera camera in cameras)
        {
            if (IsObjectVisibleFromCamera(camera))
            {
                isVisible = true;
                break;
            }
        }

        // Enable or disable the Renderer based on visibility
        objectRenderer.enabled = isVisible;
    }

    private bool IsObjectVisibleFromCamera(Camera camera)
    {
        // Check if the object is within the camera's frustum
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, objectRenderer.bounds);
    }
}
