using UnityEngine;

public class DeliveryItem : MonoBehaviour
{
    public float fastThrowForce = 1000f; // High force to make the object move very fast
    public GameObject target; // The object to launch toward
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from this game object.");
        }

        if (target == null)
        {
            Debug.LogError("No target assigned for the launch.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ensure the object has a collider and the target is assigned
        if (collision.collider != null && rb != null && target != null)
        {
            // Calculate the direction from this object to the target
            Vector3 direction = (target.transform.position - transform.position).normalized;

            // Reset velocity to avoid combining forces
            rb.velocity = Vector3.zero;

            // Apply a strong force toward the target
            rb.AddForce(direction * fastThrowForce, ForceMode.Impulse);

            // Debug message to see what was hit and where it's being launched
            Debug.Log("Collided with: " + collision.collider.name + ". Launching toward: " + target.name);
        }
    }
}
