using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SubtitlesManager : MonoBehaviour
{
    public Text subtitleText;
    public List<AudioClip> audioClips; // List of audio clips
    public List<string> subtitles; // List of corresponding subtitles

    private Dictionary<AudioClip, string> subtitlesDictionary = new Dictionary<AudioClip, string>();
    private bool subtitlesEnabled = false;
    private List<AudioSource> audioSources = new List<AudioSource>();
    private Coroutine checkAudioCoroutine;
    private float checkInterval = 0.5f; // Check every 0.5 seconds

    void Start()
    {
        LoadSubtitlesSettings();
        subtitleText.enabled = subtitlesEnabled;

        // Populate the subtitles dictionary
        for (int i = 0; i < audioClips.Count; i++)
        {
            if (i < subtitles.Count && audioClips[i] != null)
            {
                AddSubtitle(audioClips[i], subtitles[i]);
            }
        }

        // Start the coroutine to check for playing audio sources
        if (subtitlesEnabled)
        {
            checkAudioCoroutine = StartCoroutine(CheckAudioSources());
        }
    }

    IEnumerator CheckAudioSources()
    {
        while (subtitlesEnabled)
        {
            UpdateAudioSourcesList(); // Update the list of audio sources

            bool subtitleDisplayed = false;
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource.isPlaying && audioSource.clip != null && subtitlesDictionary.ContainsKey(audioSource.clip))
                {
                    subtitleText.text = subtitlesDictionary[audioSource.clip];
                    subtitleText.enabled = true;
                    subtitleDisplayed = true;
                    break;
                }
            }

            if (!subtitleDisplayed)
            {
                subtitleText.enabled = false;
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    void UpdateAudioSourcesList()
    {
        // Find all audio sources in the scene and update the list
        audioSources.Clear();
        audioSources.AddRange(FindObjectsOfType<AudioSource>());
    }

    public void AddSubtitle(AudioClip clip, string subtitle)
    {
        if (clip != null && !subtitlesDictionary.ContainsKey(clip))
        {
            subtitlesDictionary.Add(clip, subtitle);
        }
    }

    public void ToggleSubtitles(bool enabled)
    {
        subtitlesEnabled = enabled;
        subtitleText.enabled = enabled;
        PlayerPrefs.SetInt("SubtitlesEnabled", enabled ? 1 : 0);

        if (subtitlesEnabled)
        {
            // Start the coroutine if subtitles are enabled
            checkAudioCoroutine = StartCoroutine(CheckAudioSources());
        }
        else
        {
            // Stop the coroutine if subtitles are disabled
            if (checkAudioCoroutine != null)
            {
                StopCoroutine(checkAudioCoroutine);
                checkAudioCoroutine = null;
            }
            subtitleText.enabled = false; // Ensure subtitle text is hidden
        }
    }

    private void LoadSubtitlesSettings()
    {
        subtitlesEnabled = PlayerPrefs.GetInt("SubtitlesEnabled", 1) == 1;
    }
}
