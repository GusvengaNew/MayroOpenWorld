using UnityEngine;
using UnityEngine.UI;

public class SubtitleToggle : MonoBehaviour
{
    public Toggle subtitleToggle;

    void Start()
    {
        subtitleToggle.isOn = PlayerPrefs.GetInt("SubtitlesEnabled", 1) == 1;
        subtitleToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("SubtitlesEnabled", isOn ? 1 : 0);
    }
}
