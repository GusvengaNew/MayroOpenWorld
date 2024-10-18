using UnityEngine;
using UnityEngine.AI;

public class SimpleNPC : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    private NavMeshAgent agent; // Reference to the NavMeshAgent component

    void Start()
    {
        // Get the NavMeshAgent component attached to this GameObject
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check if the player is assigned
        if (player != null)
        {
            // Set the agent's destination to the player's position
            agent.SetDestination(player.position);
        }
    }
}
