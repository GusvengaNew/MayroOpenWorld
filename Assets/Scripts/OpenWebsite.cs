using UnityEngine;
using UnityEngine.UI;

public class OpenWebsite : MonoBehaviour
{
    // URL to open
    public string url = "https://www.example.com";

    // Reference to the Button component
    private Button button;

    void Start()
    {
        // Get the Button component attached to this GameObject
        button = GetComponent<Button>();

        // Add a listener to call the OpenLink method when the button is clicked
        if (button != null)
        {
            button.onClick.AddListener(OpenLink);
        }
    }

    void OpenLink()
    {
        // Open the URL in the default web browser
        Application.OpenURL(url);
    }
}
