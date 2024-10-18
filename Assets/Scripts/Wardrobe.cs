using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    public Animator animator;            // Reference to the Animator component
    public float interactionRange = 2.0f; // Range within which the player can interact with the wardrobe

    private bool isPlayerNear = false;   // Tracks if the player is near the wardrobe
    private bool isOpen = false;         // Tracks if the wardrobe is currently open

    private Transform player;            // Reference to the player's transform

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Assuming the player has a tag "Player"
        player = GameObject.FindWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    private void Update()
    {
        // Check if the player is within interaction range
        if (Vector3.Distance(player.position, transform.position) <= interactionRange)
        {
            isPlayerNear = true;
        }
        else
        {
            isPlayerNear = false;
        }

        // If the player is near and presses the E key
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (isOpen)
            {
                // Close the wardrobe
                animator.SetTrigger("Close");
                isOpen = false;
            }
            else
            {
                // Open the wardrobe
                animator.SetTrigger("Open");
                isOpen = true;
            }
        }

        // Ensure the correct idle state is applied after the animation completes
        if (isOpen && animator.GetCurrentAnimatorStateInfo(0).IsName("wardrobe_open"))
        {
            animator.SetTrigger("Idle");
        }
        else if (!isOpen && animator.GetCurrentAnimatorStateInfo(0).IsName("wardrobe_close"))
        {
            animator.SetTrigger("Idle");
        }
    }

    // Debugging: Show the interaction range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
