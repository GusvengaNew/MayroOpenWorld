using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToggleInfo : MonoBehaviour
{
    public Toggle sceneToggle; // The UI toggle object
    private string toggleKey = "SkipInfoState"; // Key to store toggle state in PlayerPrefs

    void Start()
    {
        // Set the toggle to its saved state or default to off if no saved state exists
        if (PlayerPrefs.HasKey(toggleKey))
        {
            bool isToggled = PlayerPrefs.GetInt(toggleKey) == 1;
            sceneToggle.isOn = isToggled;
        }
        else
        {
            sceneToggle.isOn = false; // Default to off
        }

        // Subscribe to the toggle's onValueChanged event
        sceneToggle.onValueChanged.AddListener(OnToggleChanged);

        // Check if the toggle is on and skip the first scene if needed
        SkipFirstSceneIfToggled();
    }

    // This function is called whenever the toggle's value changes
    public void OnToggleChanged(bool isOn)
    {
        // Save the toggle state to PlayerPrefs
        PlayerPrefs.SetInt(toggleKey, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Checks the toggle state and skips the first scene if the toggle is on
    private void SkipFirstSceneIfToggled()
    {
        bool isToggled = PlayerPrefs.GetInt(toggleKey, 0) == 1; // Default is 0 (off)

        // If toggled on and we are in the first scene, skip to the second scene
        if (isToggled && SceneManager.GetActiveScene().buildIndex == 0)
        {
            // Assumes the next scene in Build Settings is the one to load
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
