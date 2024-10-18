using UnityEngine;

public class BooserBullet : MonoBehaviour
{
    public float moveSpeed = 25f; // Speed at which the object moves

    private Vector3 moveDirection;
    private bool directionCaptured = false;

    void Start()
    {
        // Find the player object using the "Player" tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Calculate the direction vector from the current position to the initial player position
            moveDirection = (player.transform.position - transform.position).normalized;
            directionCaptured = true;
        }
        else
        {
            Debug.LogError("No GameObject with the tag 'Player' was found in the scene.");
        }
    }

    void Update()
    {
        if (directionCaptured)
        {
            // Move in the direction of the initial player position
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
}
