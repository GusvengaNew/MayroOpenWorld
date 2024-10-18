using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FakeDownload : MonoBehaviour
{
    public Text downloadText;
    public Slider downloadSlider;
    public Button targetButton;
    public float goalInBytes = 69 * Mathf.Pow(10, 15); // 69 Petabytes in Bytes
    public float textUpdateSpeed = 0.1f; // Speed of text update in seconds
    public float sliderUpdateSpeed = 0.1f; // Speed of slider update in seconds
    public float minIncrement = 1e9f; // Minimum increment (1 GB)
    public float maxIncrement = 5e9f; // Maximum increment (5 GB)
    public float minSkipIncrement = 1e10f; // Minimum skip increment (10 GB)
    public float maxSkipIncrement = 5e10f; // Maximum skip increment (50 GB)

    private float currentDownloadBytes = 0;
    private bool isPaused = false;

    void Start()
    {
        targetButton.interactable = false; // Make sure the button is initially disabled
        StartCoroutine(UpdateDownloadStatus());
        StartCoroutine(UpdateSliderValue());
    }

    IEnumerator UpdateDownloadStatus()
    {
        while (currentDownloadBytes < goalInBytes)
        {
            if (!isPaused)
            {
                if (Random.Range(0, 10) > 7) // Random chance to skip
                {
                    currentDownloadBytes += Random.Range(minSkipIncrement, maxSkipIncrement); // Skip increment
                }
                else
                {
                    currentDownloadBytes += Random.Range(minIncrement, maxIncrement); // Normal increment
                }
                
                if (currentDownloadBytes > goalInBytes)
                    currentDownloadBytes = goalInBytes;

                downloadText.text = FormatBytes(currentDownloadBytes) + " / 69 PB";
            }
            
            if (Random.Range(0, 10) > 8) // Random chance to pause
            {
                isPaused = true;
                yield return new WaitForSeconds(Random.Range(1, 4)); // Pause for 1 to 3 seconds
                isPaused = false;
            }

            yield return new WaitForSeconds(textUpdateSpeed); // Update based on the customizable speed
        }

        CheckCompletion();
    }

    IEnumerator UpdateSliderValue()
    {
        while (downloadSlider.value < 1)
        {
            downloadSlider.value += Random.Range(0.001f, 0.01f); // Increment slider value randomly
            if (downloadSlider.value > 1)
                downloadSlider.value = 1;

            yield return new WaitForSeconds(sliderUpdateSpeed); // Update based on the customizable speed
        }

        CheckCompletion();
    }

    void CheckCompletion()
    {
        if (currentDownloadBytes >= goalInBytes && downloadSlider.value >= 1)
        {
            targetButton.interactable = true; // Enable the button
        }
    }

    string FormatBytes(float bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB", "PB" };
        int order = 0;
        while (bytes >= 1024 && order < sizes.Length - 1)
        {
            order++;
            bytes = bytes / 1024;
        }
        return string.Format("{0:0.##} {1}", bytes, sizes[order]);
    }
}
