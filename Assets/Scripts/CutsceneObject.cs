using UnityEngine;

public class CutsceneObject : MonoBehaviour
{
    public float fastThrowForce = 1000f; // High force to make the object move very fast
    public string targetTag = "Enemy"; // Tag that will trigger the fast throw
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from this game object.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object's tag matches the target tag
        if (collision.collider.CompareTag(targetTag))
        {
            if (rb != null)
            {
                // Apply a strong force in the direction of the collision normal
                rb.velocity = Vector3.zero; // Reset velocity to avoid combining forces
                rb.AddForce(collision.contacts[0].normal * fastThrowForce, ForceMode.Impulse);
            }
        }
    }
}
