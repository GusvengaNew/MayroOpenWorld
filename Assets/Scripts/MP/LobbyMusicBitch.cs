using UnityEngine;
using UnityEngine.Networking;

public class LobbyMusicBitch : NetworkBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public AudioClip specialAudioClip; // Special audio clip for the 69th play

    private int playCount = 0; // Track the number of plays

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found. Please attach an AudioSource to the GameObject or assign it manually.");
                return;
            }
        }

        if (audioClips.Length == 0)
        {
            Debug.LogError("No audio clips assigned. Please assign one or more audio clips to the array.");
            return;
        }

        if (isServer)
        {
            RpcPlayRandomAudio(); // Play the first audio immediately and sync with clients
        }
    }

    void Update()
    {
        if (isServer && !audioSource.isPlaying)
        {
            RpcPlayRandomAudio();
        }
    }

    [ClientRpc]
    void RpcPlayRandomAudio()
    {
        if (audioClips.Length > 0)
        {
            playCount++; // Increment the play count

            if (playCount == 69 && specialAudioClip != null)
            {
                // Play the special audio clip on the 69th play
                audioSource.clip = specialAudioClip;
            }
            else
            {
                int randomIndex = Random.Range(0, audioClips.Length);
                audioSource.clip = audioClips[randomIndex];
            }

            // Enable the audio source if it is disabled
            if (!audioSource.enabled)
            {
                audioSource.enabled = true;
            }

            audioSource.Play();

            if (isServer)
            {
                // Schedule the next audio to play immediately after the current one finishes
                Invoke("RpcPlayRandomAudio", audioSource.clip.length);
            }
        }
    }
}
