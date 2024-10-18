using UnityEngine;
using UnityEngine.AI;

public class BooserNPC : MonoBehaviour
{
    public Transform[] waypoints;
    public float waypointRadius = 5f;
    public float chaseRadius = 20f;
    public float losePlayerRadius = 30f;
    public float wanderSpeed = 3.5f;
    public float chaseSpeed = 7f;
    public float fieldOfView = 60f;
    public AudioSource audioSource;
    public AudioSource lostPlayerAudioSource;
    public AudioSource chaseLoopAudioSource;
    public AudioClip[] chaseSounds;
    public AudioClip[] wanderSounds;
    public AudioClip[] lostPlayerSounds;

    public float minTimeBetweenWanderSounds = 5f;
    public float maxTimeBetweenWanderSounds = 15f;
    public float minTimeBetweenChaseSounds = 3f;
    public float maxTimeBetweenChaseSounds = 7f;

    public GameObject detectionBoostObject;
    public AudioClip[] reactiveSounds;

    private Transform player;
    private NavMeshAgent agent;
    private bool isChasing = false;
    private bool wasChasing = false;
    private bool isRespondingToSound = false;
    private bool sawPlayerAfterSoundResponse = false;
    private Vector3 lastKnownPlayerPosition;
    private float nextWanderSoundTime;
    private float nextChaseSoundTime;
    private float resumeWanderingTime;
    private bool isReactingToSound = false;  // Flag to indicate sound reaction

    private float originalWanderSpeed;
    private int lastChaseSoundIndex = -1;
    private int lastWanderSoundIndex = -1;
    private int lastLostPlayerSoundIndex = -1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalWanderSpeed = wanderSpeed;
        SetRandomRoamingDestination();
        ScheduleNextWanderSound();
    }

    void Update()
    {
        if (agent == null || player == null) return;

        float currentChaseRadius = chaseRadius;
        float currentLosePlayerRadius = losePlayerRadius;

        if (detectionBoostObject != null && Vector3.Distance(player.position, detectionBoostObject.transform.position) <= chaseRadius)
        {
            currentChaseRadius *= 2f;
            currentLosePlayerRadius *= 2f;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (CanSeePlayer(distanceToPlayer, currentChaseRadius))
        {
            sawPlayerAfterSoundResponse = isRespondingToSound; // Check if player is spotted after sound response
            isRespondingToSound = false; // Reset sound response state
            agent.speed = chaseSpeed; // Always use chase speed when chasing

            if (!isChasing)
            {
                isChasing = true;
                PlayRandomChaseSound(true);
                ScheduleNextChaseSound();

                if (chaseLoopAudioSource != null)
                {
                    chaseLoopAudioSource.loop = true;
                    chaseLoopAudioSource.Play();
                }
            }

            SetDestination(player.position);
            lastKnownPlayerPosition = player.position;

            if (Time.time >= nextChaseSoundTime)
            {
                PlayRandomChaseSound();
                ScheduleNextChaseSound();
            }
        }
        else if (isRespondingToSound)
        {
            // Continue to move towards the sound even if it's reached
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!isReactingToSound)
                {
                    isReactingToSound = true;
                    agent.speed = chaseSpeed; // Speed up when reacting to sound
                }
                SetDestination(transform.position + (transform.position - lastKnownPlayerPosition).normalized * 5f); // Move away from the destination
            }
            else
            {
                isReactingToSound = false; // Reset flag after movement
            }
        }
        else if (isChasing && distanceToPlayer > currentLosePlayerRadius)
        {
            isChasing = false;
            wasChasing = true;
            agent.speed = wanderSpeed;

            if (chaseLoopAudioSource != null)
            {
                chaseLoopAudioSource.Stop();
            }

            SetDestination(lastKnownPlayerPosition);
        }

        if (wasChasing && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            wasChasing = false;

            PlayRandomLostPlayerSound();

            resumeWanderingTime = Time.time + lostPlayerAudioSource.clip.length;
        }

        if (sawPlayerAfterSoundResponse && !isChasing && !wasChasing)
        {
            agent.speed = originalWanderSpeed; // Reset speed to normal after seeing player post-sound response
            sawPlayerAfterSoundResponse = false; // Reset flag
        }

        if (Time.time >= resumeWanderingTime && !lostPlayerAudioSource.isPlaying && !isChasing && agent.isStopped)
        {
            agent.isStopped = false;
            SetRandomRoamingDestination();
        }

        if (!isChasing && !wasChasing && Time.time >= nextWanderSoundTime)
        {
            PlayRandomWanderSound();
            ScheduleNextWanderSound();
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetRandomRoamingDestination();
        }
    }

    bool CanSeePlayer(float distanceToPlayer, float adjustedChaseRadius)
    {
        if (distanceToPlayer > adjustedChaseRadius) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleBetweenNPCAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleBetweenNPCAndPlayer <= fieldOfView / 2f)
        {
            return true;
        }

        return false;
    }

    public void ReactToSound(Vector3 soundLocation)
    {
        if (isChasing) return;

        isChasing = false;
        wasChasing = false;
        isRespondingToSound = true;

        if (chaseLoopAudioSource != null && chaseLoopAudioSource.isPlaying)
        {
            chaseLoopAudioSource.Stop();
        }

        agent.speed = chaseSpeed; // Switch to chase speed when reacting to sound
        SetDestination(soundLocation);
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
            agent.isStopped = false;
            agent.speed = wanderSpeed;
            agent.SetDestination(hit.position);
        }
    }

    void SetDestination(Vector3 destination)
    {
        if (agent == null) return;

        agent.isStopped = false;
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
        lostPlayerAudioSource.Play();
        agent.isStopped = true;
    }

    int PlayRandomSoundFromSet(AudioClip[] soundSet, int lastIndex, AudioSource source = null)
    {
        if (source == null) source = audioSource;

        if (soundSet.Length > 0 && source != null)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, soundSet.Length);
            } while (randomIndex == lastIndex && soundSet.Length > 1);

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

    public bool IsReactiveSound(AudioClip clip)
    {
        foreach (var reactiveClip in reactiveSounds)
        {
            if (clip == reactiveClip)
            {
                return true;
            }
        }
        return false;
    }

    public void OnSoundPlayed(AudioClip clip, Vector3 soundLocation)
    {
        if (IsReactiveSound(clip))
        {
            ReactToSound(soundLocation);
        }
    }

    public bool CanSeeObject(Vector3 objectPosition)
    {
        Vector3 directionToObject = (objectPosition - transform.position).normalized;
        float angleBetweenNPCAndObject = Vector3.Angle(transform.forward, directionToObject);

        float distanceToObject = Vector3.Distance(transform.position, objectPosition);

        if (angleBetweenNPCAndObject <= fieldOfView / 2f && distanceToObject <= chaseRadius)
        {
            return true;
        }
        return false;
    }
}
