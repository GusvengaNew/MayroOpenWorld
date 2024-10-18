using UnityEngine;
using UnityEngine.UI;

public class SplashTexts : MonoBehaviour
{
    public Text displayText; // Reference to the UI Text component
    public string[] randomTexts; // Array of strings to choose from

    void Start()
    {
        if (randomTexts.Length > 0)
        {
            DisplayRandomText();
        }
        else
        {
            Debug.LogError("RandomTexts array is empty. Please add some texts in the inspector.");
        }
    }

    void DisplayRandomText()
    {
        int randomIndex = Random.Range(0, randomTexts.Length);
        displayText.text = randomTexts[randomIndex];
    }

    // You can also call this method to change the text at any time
    public void ChangeText()
    {
        DisplayRandomText();
    }
}
