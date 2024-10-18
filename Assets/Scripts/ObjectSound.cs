using UnityEngine;

public class ObjectSound : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip[] audioClips; // Array of audio clips to choose from

    private bool hasSettled = false; // Flag to indicate if the object has settled

    private void Start()
    {
        // Set a delay before allowing collision sounds to play
        Invoke("Settle", 1.0f); // Adjust the delay as needed
    }

    private void Settle()
    {
        hasSettled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasSettled)
            return;

        // Play a random audio clip from the array
        if (audioClips.Length > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[randomIndex];
            audioSource.Play();
        }
    }
}
