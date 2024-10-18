using UnityEngine;

public class DynamicLOD : MonoBehaviour
{
    public Mesh highDetailMesh;   // The high detail mesh
    public Mesh mediumDetailMesh; // The medium detail mesh
    public Mesh lowDetailMesh;    // The low detail mesh

    public float highDetailDistance = 20f;  // The distance at which the high detail mesh is used
    public float mediumDetailDistance = 50f; // The distance at which the medium detail mesh is used
    public float lowDetailDistance = 100f;  // The distance at which the low detail mesh is used

    private MeshFilter meshFilter;
    private Camera mainCamera;

    void Start()
    {
        // Get the MeshFilter component and the main camera
        meshFilter = GetComponent<MeshFilter>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Calculate the distance between the object and the camera
        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);

        // Switch the mesh based on the distance
        if (distance < highDetailDistance)
        {
            meshFilter.mesh = highDetailMesh;
        }
        else if (distance < mediumDetailDistance)
        {
            meshFilter.mesh = mediumDetailMesh;
        }
        else if (distance < lowDetailDistance)
        {
            meshFilter.mesh = lowDetailMesh;
        }
    }
}
