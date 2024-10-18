using UnityEngine;
using UnityEngine.UI;

public class TogglePP : MonoBehaviour
{
    public Toggle toggle;          // Reference to the UI Toggle
    public GameObject targetObject; // The object to be activated/deactivated
    private static TogglePP instance;

    private void Awake()
    {
        // Ensure only one instance of this object exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Check if PlayerPrefs has the key "ObjectActive"
        if (PlayerPrefs.HasKey("ObjectActive"))
        {
            // Load the saved state and set the toggle accordingly
            bool isActive = PlayerPrefs.GetInt("ObjectActive") == 1;
            toggle.isOn = isActive;
            targetObject.SetActive(isActive);
        }
        else
        {
            // If no PlayerPrefs found, set the default state to off
            toggle.isOn = false;
            targetObject.SetActive(false);
        }
    }

    private void Start()
    {
        // Add listener to the toggle
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        // Activate/deactivate the target object based on the toggle state
        targetObject.SetActive(isOn);

        // Save the state in PlayerPrefs
        PlayerPrefs.SetInt("ObjectActive", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}
