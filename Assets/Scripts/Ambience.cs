using UnityEngine;

public class Ambience : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public float minTimeBetweenPlays = 5f;
    public float maxTimeBetweenPlays = 15f;

    private float nextPlayTime;

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

        CalculateNextPlayTime();
    }

    void Update()
    {
        if (Time.time >= nextPlayTime)
        {
            PlayRandomAudio();
            CalculateNextPlayTime();
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
