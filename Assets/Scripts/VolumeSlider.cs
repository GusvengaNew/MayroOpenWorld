using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the UI Slider
    public Text volumeText; // Reference to the UI Text
    private const string VolumePrefKey = "GameVolume"; // PlayerPrefs key for storing the volume

    void Start()
    {
        // Load the saved volume value from PlayerPrefs
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1.0f); // Default volume is 1.0 (max)
        volumeSlider.value = savedVolume;
        UpdateVolume(savedVolume);
        
        // Add listener for slider value changes
        volumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    // This function is called when the slider value is changed
    public void OnSliderValueChanged()
    {
        float volume = volumeSlider.value;
        UpdateVolume(volume);
        
        // Save the current volume to PlayerPrefs
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
    }

    // Update the game volume and the volume text
    private void UpdateVolume(float volume)
    {
        // Set the global volume (0.0 to 1.0)
        AudioListener.volume = volume;
        
        // Update the volume text to show the current volume value as a percentage
        volumeText.text = (volume * 100).ToString("0") + "%";
    }
}
