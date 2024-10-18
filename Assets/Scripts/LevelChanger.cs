using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    // Singleton instance
    private static LevelChanger instance;

    // Reference to the AudioSource component
    public AudioSource audioSource;

    // Reference to the audio clip to be played
    public AudioClip audioClip;

    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;
            // Set this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // If instance already exists and it's not this, destroy this to enforce the singleton pattern
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Check if Left Ctrl + X + C is pressed for the next scene
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.X) && Input.GetKeyDown(KeyCode.C))
        {
            ChangeToNextScene();
        }

        // Check if Right Ctrl + X + C is pressed for the previous scene
        if (Input.GetKey(KeyCode.RightControl) && Input.GetKey(KeyCode.X) && Input.GetKeyDown(KeyCode.C))
        {
            ChangeToPreviousScene();
        }
    }

    void ChangeToNextScene()
    {
        // Play sound
        PlaySound();
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();
        // Calculate the next scene index
        int nextSceneIndex = (currentScene.buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        // Load the next scene
        SceneManager.LoadScene(nextSceneIndex);
    }

    void ChangeToPreviousScene()
    {
        // Play sound
        PlaySound();
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();
        // Calculate the previous scene index
        int previousSceneIndex = currentScene.buildIndex - 1;
        // If we're at the first scene, wrap around to the last scene
        if (previousSceneIndex < 0)
        {
            previousSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        }
        // Load the previous scene
        SceneManager.LoadScene(previousSceneIndex);
    }

    void PlaySound()
    {
        // Check if the audio source and audio clip are not null and play the sound
        if (audioSource != null && audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is not assigned.");
        }
    }
}
