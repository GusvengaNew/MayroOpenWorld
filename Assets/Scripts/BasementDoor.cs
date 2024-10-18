using UnityEngine;

public class BasementDoor : MonoBehaviour
{
    // The two objects that need to collide with this object
    public GameObject object1;
    public GameObject object2;

    // The object that will be enabled after both collisions
    public GameObject targetObject;

    // Flags to check if each object has collided
    private bool hasObject1Collided = false;
    private bool hasObject2Collided = false;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the first object has collided
        if (collision.gameObject == object1)
        {
            hasObject1Collided = true;
            Debug.Log("Object 1 has collided.");
        }

        // Check if the second object has collided
        if (collision.gameObject == object2)
        {
            hasObject2Collided = true;
            Debug.Log("Object 2 has collided.");
        }

        // If both objects have collided
        if (hasObject1Collided && hasObject2Collided)
        {
            // Enable the target object
            targetObject.SetActive(true);
            
            // Disable this object
            gameObject.SetActive(false);

            Debug.Log("Both objects have collided. Target object enabled, current object disabled.");
        }
    }
}
