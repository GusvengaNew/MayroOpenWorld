using UnityEngine;

public class DebugNames : MonoBehaviour
{
    private bool isVisualizing = false;
    private GameObject[] allObjects; // Array to store all objects in the scene
    private GameObject[] nameTags; // Array to store the name tags

    void Start()
    {
        // Get all active GameObjects in the scene
        allObjects = FindObjectsOfType<GameObject>();
        nameTags = new GameObject[allObjects.Length]; // To store name tag references
    }

    void Update()
    {
        // Toggle visualization on or off when "3" key is pressed
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isVisualizing = !isVisualizing;

            if (isVisualizing)
            {
                AttachNameTags();
            }
            else
            {
                DetachNameTags();
            }
        }
    }

    // Method to attach 3D text with the object's name
    void AttachNameTags()
    {
        for (int i = 0; i < allObjects.Length; i++)
        {
            GameObject obj = allObjects[i];

            if (nameTags[i] == null) // Avoid attaching duplicate name tags
            {
                // Calculate the number of children and adjust position accordingly
                int childCount = obj.transform.childCount;

                // Create a new GameObject for the name tag
                GameObject nameTag = new GameObject("NameTag");
                nameTag.transform.SetParent(obj.transform);

                // Position the tag above the object, stacking texts if it has children
                nameTag.transform.localPosition = Vector3.up * (2 + childCount * 0.5f);

                // Add TextMesh component to display the object's name
                TextMesh textMesh = nameTag.AddComponent<TextMesh>();
                textMesh.text = obj.name; // Set the object's name as the text
                textMesh.characterSize = 0.2f; // Set the text size
                textMesh.anchor = TextAnchor.MiddleCenter; // Center the text
                textMesh.alignment = TextAlignment.Center; // Align text in the middle

                // Store reference to the created name tag
                nameTags[i] = nameTag;
            }
        }
    }

    // Method to remove the 3D text name tags
    void DetachNameTags()
    {
        for (int i = 0; i < nameTags.Length; i++)
        {
            if (nameTags[i] != null)
            {
                Destroy(nameTags[i]); // Remove the name tag from the scene
                nameTags[i] = null; // Clear reference to the destroyed name tag
            }
        }
    }
}
