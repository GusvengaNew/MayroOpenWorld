using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EngineSound : MonoBehaviour
{
    public Rigidbody targetRigidbody; // The Rigidbody of the object to track speed
    public float minPitch = 0.5f; // Minimum pitch value
    public float maxPitch = 2.0f; // Maximum pitch value
    public float maxSpeed = 20.0f; // Speed at which the pitch will be at maxPitch

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (targetRigidbody == null)
        {
            Debug.LogError("No Rigidbody attached to the SpeedBasedPitch script!");
        }
    }

    void Update()
    {
        if (targetRigidbody != null)
        {
            float speed = targetRigidbody.velocity.magnitude;
            float pitch = Mathf.Lerp(minPitch, maxPitch, speed / maxSpeed);
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, pitch, Time.deltaTime);
        }
    }
}
