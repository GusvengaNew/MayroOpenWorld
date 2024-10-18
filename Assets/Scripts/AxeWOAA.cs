using UnityEngine;
using System.Collections;

public class AxeWOAA : MonoBehaviour
{
    // The AudioSource to be disabled
    public AudioSource audioSource;

    // Time in seconds after which the AudioSource will be disabled
    public float timeToDisable = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (audioSource == null)
        {
            // If no AudioSource is assigned, try to get the one attached to the GameObject
            audioSource = GetComponent<AudioSource>();
        }

        // Start the coroutine to disable the AudioSource after the specified time
        if (audioSource != null)
        {
            StartCoroutine(DisableAudioSourceAfterDelay());
        }
        else
        {
            Debug.LogError("No AudioSource found on this GameObject.");
        }
    }

    // Coroutine to disable the AudioSource after the delay
    private IEnumerator DisableAudioSourceAfterDelay()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(timeToDisable);

        // Disable the AudioSource component
        audioSource.enabled = false;
    }
}
