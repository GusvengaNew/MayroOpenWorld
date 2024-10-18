using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    public string collisionTag = "Player"; // The tag that triggers the shattering

    public GameObject shatteredPrefab; // Prefab of shattered window pieces

    private bool hasShattered = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasShattered && collision.gameObject.CompareTag(collisionTag))
        {
            Shatter();
        }
    }

    private void Shatter()
    {
        hasShattered = true;

        // Instantiate the shattered window prefab
        GameObject shatteredWindow = Instantiate(shatteredPrefab, transform.position, transform.rotation);

        // Transfer the same velocity and angular velocity to shattered pieces
        Rigidbody[] rigidbodies = shatteredWindow.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.velocity = GetComponent<Rigidbody>().velocity;
            rb.angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        }

        // Destroy the original window object
        Destroy(gameObject);
    }
}
