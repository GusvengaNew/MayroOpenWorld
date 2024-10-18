using UnityEngine;
using System.Collections;

public class NoMusicArea : MonoBehaviour
{
    public AudioSource targetAudioSource; // The shared AudioSource to modify
    public float targetVolume = 1.0f; // The volume to reach when the player enters the area
    public float transitionDuration = 1.0f; // The duration of the volume transition

    private float originalVolume; // To store the original volume of the AudioSource
    private Coroutine volumeCoroutine; // To handle the volume transition coroutine

    void Start()
    {
        // Store the original volume of the AudioSource
        if (targetAudioSource != null)
        {
            originalVolume = targetAudioSource.volume;
        }
        else
        {
            Debug.LogError("Target AudioSource is not assigned.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the area
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the area");
            if (volumeCoroutine != null)
            {
                StopCoroutine(volumeCoroutine);
            }
            volumeCoroutine = StartCoroutine(ChangeVolume(targetAudioSource.volume, targetVolume));
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player leaves the area
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the area");
            if (volumeCoroutine != null)
            {
                StopCoroutine(volumeCoroutine);
            }
            volumeCoroutine = StartCoroutine(ChangeVolume(targetAudioSource.volume, originalVolume));
        }
    }

    private IEnumerator ChangeVolume(float fromVolume, float toVolume)
    {
        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float newVolume = Mathf.Lerp(fromVolume, toVolume, elapsed / transitionDuration);
            Debug.Log("Changing volume to: " + newVolume);
            targetAudioSource.volume = newVolume;
            yield return null;
        }
        targetAudioSource.volume = toVolume; // Ensure the final volume is set
        Debug.Log("Final volume set to: " + toVolume);
    }
}
