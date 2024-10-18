using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    public float speed = 5f;  // Speed of the object's movement
    public float lifetime = 2f;  // Time in seconds before the object disappears

    private float elapsedTime = 0f;

    void Update()
    {
        // Move the object in the negative X direction at the specified speed
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Update the elapsed time
        elapsedTime += Time.deltaTime;

        // Check if the object's lifetime has been exceeded
        if (elapsedTime >= lifetime)
        {
            // Destroy the object
            NetworkServer.Destroy(gameObject);
        }
    }
}
