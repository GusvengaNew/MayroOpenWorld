using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomPitch : MonoBehaviour
{
    private AudioSource audioSource;

    // Set the range for random pitch
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    void Awake()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        // Randomize pitch between the specified range
        audioSource.pitch = Random.Range(minPitch, maxPitch);

        // Play the audio if the AudioSource is enabled
        if (audioSource.isActiveAndEnabled)
        {
            audioSource.Play();
        }
    }
}
