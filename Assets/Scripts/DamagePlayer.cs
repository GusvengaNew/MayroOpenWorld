using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip[] audioClips;  // Array of audio clips to choose from
    public string playerTag = "Player"; // Tag to identify the player object

    void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is not assigned.");
        }
        if (audioClips.Length == 0)
        {
            Debug.LogError("No audio clips assigned.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && !audioSource.isPlaying)
        {
            PlayRandomSound();
        }
    }

    void PlayRandomSound()
    {
        if (audioClips.Length > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[randomIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clips available to play.");
        }
    }
}
