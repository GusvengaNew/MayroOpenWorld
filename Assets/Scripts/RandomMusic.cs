using UnityEngine;

public class RandomMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public float minTimeBetweenPlays = 5f;
    public float maxTimeBetweenPlays = 15f;
    public float initialPlayDelay = 2f; // New variable for the initial play delay

    private float nextPlayTime;
    private bool firstPlay = true; // Track if it's the first play

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found. Please attach an AudioSource to the GameObject or assign it manually.");
            }
        }

        if (audioClips.Length == 0)
        {
            Debug.LogError("No audio clips assigned. Please assign one or more audio clips to the array.");
        }

        nextPlayTime = Time.time + initialPlayDelay; // Set the next play time to the initial delay
    }

    void Update()
    {
        if (Time.time >= nextPlayTime)
        {
            PlayRandomAudio();
            if (firstPlay)
            {
                firstPlay = false; // Mark that the first play has happened
                CalculateNextPlayTime(); // Calculate the next play time based on the general timing logic
            }
            else
            {
                CalculateNextPlayTime(); // Calculate the next play time based on the general timing logic
            }
        }
    }

    void PlayRandomAudio()
    {
        if (audioClips.Length > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[randomIndex];
            audioSource.Play();
        }
    }

    void CalculateNextPlayTime()
    {
        nextPlayTime = Time.time + Random.Range(minTimeBetweenPlays, maxTimeBetweenPlays);
    }
}
