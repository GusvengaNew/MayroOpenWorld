using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public Transform[] waypoints;
    public float wanderRadius = 10f;
    public float chaseRadius = 20f;
    public float speed = 3.5f;
    public AudioSource audioSource;
    public AudioClip[] chaseSounds;

    private Transform player;
    private NavMeshAgent agent;
    private int currentWaypointIndex;
    private bool isChasing = false;
    private bool wasChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentWaypointIndex = Random.Range(0, waypoints.Length);
        SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Update()
    {
        if (agent == null || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRadius)
        {
            if (!wasChasing) // If just started chasing
            {
                isChasing = true;
                wasChasing = true;
                PlayRandomChaseSound();
            }

            SetDestination(player.position);
        }
        else if (isChasing && distanceToPlayer > chaseRadius * 1.5f) // Add some buffer to stop chasing
        {
            isChasing = false;
            wasChasing = false;
            currentWaypointIndex = Random.Range(0, waypoints.Length);
            SetDestination(waypoints[currentWaypointIndex].position);
        }

        // Check if the agent is not chasing and has reached the destination
        if (!isChasing && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void SetDestination(Vector3 destination)
    {
        if (agent == null) return;

        agent.speed = speed;
        agent.SetDestination(destination);
    }

    void PlayRandomChaseSound()
    {
        if (chaseSounds.Length > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, chaseSounds.Length);
            audioSource.clip = chaseSounds[randomIndex];
            audioSource.Play();
        }
    }
}
