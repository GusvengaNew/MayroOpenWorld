using UnityEngine;
using UnityEngine.AI;

public class DudeNPC : MonoBehaviour
{
    public Transform[] waypoints;
    public float waypointRadius = 5f; // Radius around the waypoint for roaming
    public float chaseRadius = 20f;   // Radius within which the NPC will start chasing the player
    public float losePlayerRadius = 30f; // Radius beyond which the NPC will stop chasing the player
    public float speed = 3.5f;
    public AudioSource audioSource;
    public AudioSource lostPlayerAudioSource; // Separate audio source for lost player sounds
    public AudioClip[] chaseSounds;
    public AudioClip[] wanderSounds;
    public AudioClip[] lostPlayerSounds;

    public float minTimeBetweenWanderSounds = 5f;
    public float maxTimeBetweenWanderSounds = 15f;
    public float minTimeBetweenChaseSounds = 3f;
    public float maxTimeBetweenChaseSounds = 7f;

    private Transform player;
    private NavMeshAgent agent;
    private bool isChasing = false;
    private bool wasChasing = false; // Flag to check if NPC was previously chasing the player
    private Vector3 lastKnownPlayerPosition; // Last registered position of the player
    private float nextWanderSoundTime;
    private float nextChaseSoundTime;
    private float resumeWanderingTime; // Time to resume wandering after audio

    private float originalSpeed;

    private int lastChaseSoundIndex = -1; // Track the last played sound index
    private int lastWanderSoundIndex = -1;
    private int lastLostPlayerSoundIndex = -1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalSpeed = speed;
        SetRandomRoamingDestination();

        ScheduleNextWanderSound();
    }

    void Update()
    {
        if (agent == null || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRadius)
        {
            if (!isChasing)
            {
                isChasing = true;
                agent.speed = originalSpeed * 2;
                PlayRandomChaseSound(true); // Play initial chase sound immediately
                ScheduleNextChaseSound();
            }

            SetDestination(player.position);
            lastKnownPlayerPosition = player.position; // Update last known player position

            // Handle random chase sounds during chase
            if (Time.time >= nextChaseSoundTime)
            {
                PlayRandomChaseSound();
                ScheduleNextChaseSound();
            }
        }
        else if (isChasing && distanceToPlayer > losePlayerRadius)
        {
            // NPC loses the player
            isChasing = false;
            wasChasing = true; // Mark that NPC was previously chasing the player
            agent.speed = originalSpeed;

            // Force the NPC to move to the last known player position
            SetDestination(lastKnownPlayerPosition);
        }

        // If the NPC has reached the last known player position
        if (wasChasing && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            wasChasing = false; // Reset the wasChasing flag

            // Force the NPC to play a random lost player sound before resuming wandering
            PlayRandomLostPlayerSound();
            
            // Wait for the lost player sound to finish before wandering
            resumeWanderingTime = Time.time + lostPlayerAudioSource.clip.length; // Adjust to wait for the actual sound duration
        }

        // Resume wandering after the delay
        if (Time.time >= resumeWanderingTime && !lostPlayerAudioSource.isPlaying && !isChasing && agent.isStopped)
        {
            agent.isStopped = false; // Resume moving
            SetRandomRoamingDestination(); // Switch back to wandering
        }

        // Handle wander sounds
        if (!isChasing && !wasChasing && Time.time >= nextWanderSoundTime)
        {
            PlayRandomWanderSound();
            ScheduleNextWanderSound();
        }

        // Prevent getting stuck at waypoints
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetRandomRoamingDestination();
        }
    }

    void SetRandomRoamingDestination()
    {
        if (agent == null || waypoints.Length == 0) return;

        Transform selectedWaypoint = waypoints[Random.Range(0, waypoints.Length)];
        Vector3 randomDirection = Random.insideUnitSphere * waypointRadius;
        randomDirection += selectedWaypoint.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, waypointRadius, 1))
        {
            agent.isStopped = false; // Ensure the agent is not stopped
            agent.speed = speed;
            agent.SetDestination(hit.position);
        }
    }

    void SetDestination(Vector3 destination)
    {
        if (agent == null) return;

        agent.isStopped = false; // Ensure the agent is moving when setting a new destination
        agent.SetDestination(destination);
    }

    void PlayRandomChaseSound(bool immediate = false)
    {
        lastChaseSoundIndex = PlayRandomSoundFromSet(chaseSounds, lastChaseSoundIndex);

        if (immediate)
        {
            audioSource.Play();
        }
        else if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void PlayRandomWanderSound()
    {
        lastWanderSoundIndex = PlayRandomSoundFromSet(wanderSounds, lastWanderSoundIndex);
    }

    void PlayRandomLostPlayerSound()
    {
        lastLostPlayerSoundIndex = PlayRandomSoundFromSet(lostPlayerSounds, lastLostPlayerSoundIndex, lostPlayerAudioSource);
        lostPlayerAudioSource.Play(); // Play the lost player sound immediately
        agent.isStopped = true; // Stop the NPC while the sound plays
    }

    int PlayRandomSoundFromSet(AudioClip[] soundSet, int lastIndex, AudioSource source = null)
    {
        if (source == null) source = audioSource; // Default to the main audio source

        if (soundSet.Length > 0 && source != null)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, soundSet.Length);
            } while (randomIndex == lastIndex && soundSet.Length > 1); // Avoid repeating the last sound

            source.clip = soundSet[randomIndex];
            return randomIndex;
        }
        return lastIndex;
    }

    void ScheduleNextWanderSound()
    {
        nextWanderSoundTime = Time.time + Random.Range(minTimeBetweenWanderSounds, maxTimeBetweenWanderSounds);
    }

    void ScheduleNextChaseSound()
    {
        nextChaseSoundTime = Time.time + Random.Range(minTimeBetweenChaseSounds, maxTimeBetweenChaseSounds);
    }
}
