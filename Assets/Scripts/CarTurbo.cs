using UnityEngine;

public class CarTurbo : MonoBehaviour
{
    public float turboMultiplier = 2.0f; // How much faster the car goes in turbo mode
    public float transitionSpeed = 2.0f; // Speed of the transition to turbo mode
    public AudioSource turboSoundSource; // The audio source that will play the turbo sound
    private Rigidbody rb;
    private bool isTurboActive = false;
    private float currentMultiplier = 1.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found. Please attach this script to a GameObject with a Rigidbody.");
        }

        if (turboSoundSource == null)
        {
            Debug.LogError("AudioSource not found. Please assign an AudioSource in the inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            isTurboActive = true;
            if (turboSoundSource != null)
            {
                turboSoundSource.Play();
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            isTurboActive = false;
        }
    }

    void FixedUpdate()
    {
        float targetMultiplier = isTurboActive ? turboMultiplier : 1.0f;
        currentMultiplier = Mathf.Lerp(currentMultiplier, targetMultiplier, Time.fixedDeltaTime * transitionSpeed);

        rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude * currentMultiplier);
    }
}
