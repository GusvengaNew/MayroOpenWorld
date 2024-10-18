using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MusicToggle : MonoBehaviour
{
    public Toggle audioToggle; // Reference to the Toggle UI element

    private const string AudioPrefKey = "AudioEnabled";
    private List<AudioSource> musicAudioSources; // List to track AudioSource components of "Music" objects

    void Start()
    {
        musicAudioSources = new List<AudioSource>();

        // Initialize the toggle based on the saved preference, defaulting to true (on)
        bool isAudioEnabled = PlayerPrefs.GetInt(AudioPrefKey, 1) == 1;
        audioToggle.isOn = isAudioEnabled;

        // Add listener to handle the toggle value change event
        audioToggle.onValueChanged.AddListener(OnToggleValueChanged);

        // Register to the sceneLoaded event to reapply the audio setting on scene load
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Find all "Music" objects and their AudioSource components initially
        FindAllMusicObjects();
        SetAudio(isAudioEnabled);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        // Save the preference
        PlayerPrefs.SetInt(AudioPrefKey, isOn ? 1 : 0);
        PlayerPrefs.Save();

        // Apply the audio setting
        SetAudio(isOn);
    }

    private void SetAudio(bool isEnabled)
    {
        // Set active state for all tracked AudioSource components of "Music" objects
        foreach (AudioSource audioSource in musicAudioSources)
        {
            if (audioSource != null)
            {
                audioSource.enabled = isEnabled;
                if (!isEnabled)
                {
                    audioSource.Stop(); // Stop playing audio if audio is disabled
                }
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reapply the audio setting when a new scene is loaded
        bool isAudioEnabled = PlayerPrefs.GetInt(AudioPrefKey, 1) == 1;
        SetAudio(isAudioEnabled);
    }

    private void FindAllMusicObjects()
    {
        // Clear the existing list
        musicAudioSources.Clear();

        // Find all GameObjects with the "Music" tag in the current scene
        GameObject[] musicObjects = GameObject.FindGameObjectsWithTag("Music");
        foreach (GameObject musicObject in musicObjects)
        {
            // Get the AudioSource component
            AudioSource audioSource = musicObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                musicAudioSources.Add(audioSource);
            }
        }
    }

    // Optionally, if you dynamically instantiate "Music" objects during gameplay, you may need to update your list:
    // Call this method whenever you dynamically create or destroy "Music" objects.
    private void UpdateMusicObjectList()
    {
        FindAllMusicObjects();
        bool isAudioEnabled = PlayerPrefs.GetInt(AudioPrefKey, 1) == 1;
        SetAudio(isAudioEnabled);
    }
}
