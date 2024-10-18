using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MayroHead : MonoBehaviour
{
    public AudioSource audioSource;  // The AudioSource component to play sounds
    public AudioClip[] audioClips;   // Array of audio clips to choose from
    public Animator animator;        // The Animator component controlling the sprite animation
    public string idleAnimationName = "IdleAnimation";     // The name of the idle animation state
    public string startAnimationName = "StartAnimation";   // The name of the start animation state
    public string loopAnimationName = "LoopingAnimation";  // The name of the looping animation state
    public string endAnimationName = "EndAnimation";       // The name of the end animation state

    private bool isPlaying = false;  // Flag to check if an audio clip is currently playing

    void Start()
    {
        // Start with the idle animation
        animator.Play(idleAnimationName);
    }

    // This method should be called when the UI object is clicked
    public void OnClick()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlayAnimationsAndSound());
        }
    }

    private IEnumerator PlayAnimationsAndSound()
    {
        isPlaying = true;

        // Play the start animation
        animator.Play(startAnimationName);

        // Wait for the start animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Select a random audio clip
        AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];

        // Play the looping animation
        animator.Play(loopAnimationName);

        // Wait until the loop animation starts (ensuring it starts immediately after the start animation)
        yield return null;

        // Play the selected audio clip
        audioSource.clip = clip;
        audioSource.Play();

        // Wait until the audio clip finishes playing
        yield return new WaitForSeconds(clip.length);

        // Play the end animation
        animator.Play(endAnimationName);

        // Wait for the end animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Return to the idle animation
        animator.Play(idleAnimationName);

        isPlaying = false;
    }
}
